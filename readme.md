# Interoperabilidade entre python e c#  
## Descrição  

### Introdução

O projeto visa construir um framework de produtividade que facilite o processo de interoperabilidade entre essas duas linguagens. O usuário desse framework deve ser capaz de, a partir de um código em C# rodando sobre .NET Core (ou .NET >= 5), em poucas linhas, chamar funções 
escritas em módulos pyhton, sendo possível passar parâmetros de variáveis contidas no processo C# para o processo python e, obviamente, trazer 
o resultado da função executada no processo python de volta para o processo C#.  

A principal motivação desse framework é possibilitar a fácil configuração de execução de códigos de diferentes linguagens em um mesmo host, sem necessidade
de transferências de dados via http ou qualquer outro tipo de protocolo de rede, visto que isso pode causar overhead descenessário. A vantagem de trabalhar em um mesmo host
incluem: redução de custo, já que basta um servidor por exemplo, além de um aumento na segurança já que os dados não irão trafegar na rede, mas apenas na máquina em que os processos estão executando.

Possíveis aplicações incluem:  
* Tratamento de imagens em aplicações web. Já que o python é comumente utilizado para operações com imagens, utiliza-lo juntamente a uma API Web bem estruturada em C# com ASP.NET por exemplo seria bem eficaz.
* Utilização de modelos de inteligência artifical em aplicações web. Pela mesma razão anterior, python é bem utilizado em aplicações de IA, se quisermos aplicar esses modelos em operações conjuntas ao nosso Domínio em uma API em C#, uma forma de chamar essas funções seria bem conveniente.
* Integrar funcionalidades existentes em diferentes linguagens sem a necessidade de se comunicar via protocolos de rede ou reescrever as funcionalidades de uma linguagem para outra.  

### Python-Side  

A partir de agora vamos se referir a qualquer código executado em python como código Python-Side ou PS. O código PS incluso no framework será montado a partir de um template (a definir), no qual estará uma estrutura básica do código que fará a intermediação entre C# e Pyhton. Qualquer outro código PS incluso no framework será alguma modularização do código de intermediação citado anteriormente.  

Qualquer outro código PS que venha a ser executado em uma aplicação final, será o código criado pelo próprio usuário do framework, que deseja que execução seja realizada juntamente a uma execuçao de código C#.

### CSharp-Side  

A partir de agora vamos se referir a qualquer código executado em C# como código CSharp-Side ou CS. O código CS incluso no framework consistirá em uma infraestrutura de interfaces e classes abstratas para definir utilização e comportamento em auto nível das funcionalidades mais específicas, detalhes dessa infraestrutura estará na seção de documentação.  

A ideia é que essa infraestrutura possibilite a expansão deste framework para interoperabilidade com qualquer linguagem, ou seja, teremos um funcionamento genérico a base interfaces e classes abstratas que deversão ser implementadas/específicas para a linguagem desejada. Devem existir projetos separados de acordo com cada responsabilidade, por exemplo, um projeto que irá conter comportamentos genéricos da interoperabilidade (interop) e outro que consistirá na especifacação da interop com python.  

O diagrama demonstra o fluxo e responsabilidade de cada classe/interface:  
TODO: Incluir diagrama  

O código CS deve ser bem estruturado, possuir um design fixo e seguir os princípios SOLID. Estes requisitos devem ser atingidos para possibilitar um código passível de extensão e de fácil manutenção. Dar grande importância injeção de dependência (DI) visto que esse projeto visa integração com frameworks de desenvolvimento web como ASP.NET.

### Transferência de dados entre CS e PS  

Esse é um ponto de bastante importância nesse projeto, precisamos escolher uma maneira de transferir os dados do código CS para o código PS e vice-versa. Essa transferência deve ser pouco custosa, pois queremos ser melhor que o uso de protocolos de rede, e de fácil generalização, já que podemos ter diversos tipos de código usando esse framework. As transferências pensadas até agora são:  
* Via arquivo de texto
* Banco de dados local (sqlite)
* Via arquivo, usando memória mapeada em arquivo (Memory-Mapped File), porém usando uma representação binária dos dados
* ~~Via argumentos de linha de comando~~. Esse método será descartado por conta de limites no número de caractéres que podem ser passados em argumentos de linha de comando no windows.

A última opção inicialmente parece a melhor escolha em termos de custo, um arquivo com representação binária ocupa menos espaço, logo tendo um custo menor de espaço, e de tempo em relação a escrita e leitura do arquivo. Um problema aparente é o Encode e Decode dos dados de C# para Python, visto que as duas linguagens podem possuir diferentes representação binária de suas estruturas de dados, isso pode causar dificuldade extrema de implementação ou custo computacional, requer avaliação.  

Apesar do comentário acima, precisamos selecionar o método que melhor atende nossas necessidades, para isso, teremos um projeto de benchmarking de cada um desses métodos, tanto da parte PS quanto da parte CS. A partir dos resultados selecionaremos o método ideal.  

#### Arquivo de Texto

Esse método a princípio parece ser o menos interessante de todos, pois como sabemos, armazenar dados em forma textual traz consigo dois custos. Primeiro, temos o custo de espaço, já que, a representação textual de um dado geralmente é maior quando é representado em forma de texto, por exemplo: o maior inteiro de 32 bits (2147483647) ocupa 4 bytes de espaço, já em forma textual, cada dígito é um byte logo ocupando 10 bytes, mais que o dobro. Além disso, temos o custo de tempo adicional para conversões, se eu quero somar dois inteiros em python por exemplo, preciso do dado em formato int, logo preciso converter a string lida do arquivo para o inteiro que o programa utiliza e vice-versa para o caso de escrita.  

A vantagem desse método é que ele requer uma implementação bem simples. A probabilidade de escolha dele é bem baixa.  

Tabela com resultados de benchamrking¹:  
| Type    |  	Read Time(s)	| Write Time(s) |	Read Time Mean(s) | Write Time Mean(s) |
| ------- | --------------- | ------------- | ----------------- | ------------------ |
Int       |	0.0016393999999999992 |	0.009070299999999996 |	0.00016393999999999992 |	0.0009070299999999996 |
Int List  |	4.8826696 |	2.6946900000000005 |	0.48826695999999997 |	0.26946900000000007 |
Float     |	0.011441700000000665 |	0.005532200000000209 |	0.0011441700000000665 |	0.0005532200000000209 |
Float List | 	12.759683900000002 |	16.439356300000004	 | 1.2759683900000003 | 	1.6439356300000003 |  

¹ Os resultados foram gerados rodando cada código de leitura e escrita 10 vezes cada, considerando leitura/escrita do arquivo e conversão dos dados. O tempo foi medido usando a biblioteca timeit do python e os arquivos foram lidos e escritos a partir de um disco rígido.

Para a parte CS, foi usada uma biblioteca de benchmarking, a BenchmarkDotNet, e temos os seguintes resultados

``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.201
  [Host]     : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT

```
|              Method |           Mean |        Error |       StdDev |         Median |
|-------------------- |---------------:|-------------:|-------------:|---------------:|
|        WriteIntData |       357.4 μs |     18.15 μs |     53.52 μs |       332.6 μs |
|   WriteIntArrayData |   138,821.4 μs |  2,726.04 μs |  4,162.95 μs |   138,042.0 μs |
|      WriteFloatData |       376.6 μs |     18.37 μs |     54.17 μs |       350.5 μs |
| WriteFloatArrayData |   751,595.3 μs | 14,959.45 μs | 30,558.20 μs |   737,750.9 μs |
|         ReadIntData |       110.7 μs |      1.60 μs |      1.49 μs |       111.3 μs |
|    ReadIntArrayData |   284,359.6 μs |  5,596.83 μs |  8,026.81 μs |   284,162.0 μs |
|       ReadFloatData |       120.7 μs |      2.37 μs |      2.63 μs |       120.6 μs |
|  ReadFloatArrayData | 1,080,299.9 μs |  9,779.04 μs |  8,668.86 μs | 1,080,766.6 μs |


#### Banco de dados local sqlite

Resultados para parte CS:

``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.201
  [Host]     : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT


```
|              Method |                   Mean |               Error |                StdDev |                 Median |
|-------------------- |-----------------------:|--------------------:|----------------------:|-----------------------:|
|        WriteIntData |    105,529,019.5402 ns |   4,824,267.9427 ns |    13,206,355.6419 ns |    102,939,200.0000 ns |
|         ReadIntData |              0.0026 ns |           0.0063 ns |             0.0059 ns |              0.0000 ns |
|   WriteIntArrayData |  9,534,789,085.7143 ns | 133,087,616.5954 ns |   117,978,705.5839 ns |  9,513,699,450.0000 ns |
|    ReadIntArrayData |              0.0944 ns |           0.0632 ns |             0.0843 ns |              0.0730 ns |
|      WriteFloatData |    113,779,788.2828 ns |   4,527,065.2582 ns |    13,277,098.7376 ns |    110,641,800.0000 ns |
|       ReadFloatData |              0.0655 ns |           0.0606 ns |             0.0673 ns |              0.0415 ns |
| WriteFloatArrayData | 16,243,732,067.0000 ns | 847,550,764.1206 ns | 2,499,022,550.7205 ns | 16,856,396,100.0000 ns |
|  ReadFloatArrayData |              0.0210 ns |           0.0241 ns |             0.0225 ns |              0.0133 ns |

#### Arquivo binário

Resultados para parte CS:

``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19042.985 (20H2/October2020Update)
AMD Ryzen 3 2200G with Radeon Vega Graphics, 1 CPU, 4 logical and 4 physical cores
.NET SDK=5.0.201
  [Host]     : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET 5.0.4 (5.0.421.11614), X64 RyuJIT


```
|              Method |        Mean |     Error |      StdDev |      Median |
|-------------------- |------------:|----------:|------------:|------------:|
|        WriteIntData |    380.3 μs |  19.98 μs |    58.92 μs |    357.1 μs |
|         ReadIntData |    382.8 μs |   9.05 μs |    26.39 μs |    380.6 μs |
|   WriteIntArrayData | 39,404.0 μs | 777.93 μs | 1,164.36 μs | 39,440.8 μs |
|    ReadIntArrayData | 13,687.6 μs | 264.39 μs |   411.63 μs | 13,753.6 μs |
|      WriteFloatData |    384.1 μs |  19.68 μs |    57.71 μs |    362.8 μs |
|       ReadFloatData |    363.1 μs |   6.43 μs |     6.61 μs |    365.1 μs |
| WriteFloatArrayData | 52,110.9 μs | 704.36 μs |   588.18 μs | 51,791.6 μs |
|  ReadFloatArrayData | 20,131.2 μs | 398.96 μs |   729.53 μs | 20,333.8 μs |


TODO: Indicar método escolhido e porque.  

## Roadmap

O roadmap de implementação inclui os seguintes pontos:
* Criação de prova de conceito funcionando
* Benchmarking da prova de conceito
* Escolha de melhor método de transferência de dados
* Definição de diagrama de processo da infraestrutura em C#
* Criação das interfaces e classes abstratas em C#
* Criação de testes unitários genéricos utilizando as interfaces e classes abstratas
* Criação das classes concretas para interop com python em C#
* Criação de testes unitários específicos usanod as classes concretas de interop com python
* Benchmarking do interop com python
* Criação de exemplo simples em C# chamando algumas funções simples de pyhton
* Criação de exemplo C# com ASP.NET chamando algumas funções simples de pyhton
* Criação de exemplo C# com ASP.NET chamando funções de processamento de imagens ou modelo de IA em python  

## Documentação

### Projeto genérico de interop em C#

### Projeto específico de interop com Python em C#

### Exemplo simples com Console Application

### Exemplo ASP.NET Core + funçoes python simples

### Exemplo ASP.NET Core + modelo IA ou processamento de imagem em pyhton.

## Progresso  
1 Prova de Conceito - [ ]  
-- 1.1 Criar modulo simples em python - [:heavy_check_mark:]  
-- 1.2 Criar código python intermediário - [ ]  
---- 1.2.1 Troca de dados a partir de certo arquivo de texto - [:heavy_check_mark:]  
---- 1.2.2 Troca de dados a partir de um banco local sqlite - [:heavy_check_mark:]  
---- 1.2.3 Troca de dados a partir certo arquivo, através de memória mapeada em arquivo usando representação binária - [ ]  
---- 1.2.4 Troca de dados a partir de argumentos de linha de comando - [:heavy_check_mark:]  
-- 1.3 Criar código em c# que chama código python intermediário - [:heavy_check_mark:]  
---- 1.3.1 Troca de dados a partir de certo arquivo de texto - [:heavy_check_mark:]  
---- 1.3.2 Troca de dados a partir de um banco local sqlite - [:heavy_check_mark:]  
---- 1.3.3 Troca de dados a partir certo arquivo, através de memória mapeada em arquivo usando representação binária - [:heavy_check_mark:]  
---- 1.3.4 Troca de dados a partir de argumentos de linha de comando - [:heavy_check_mark:]  
-- 1.4 Benchmarking das operações - [ ]  
---- 1.4.1 Criar código de benchmarking para código python intermediário - [ ]  
---- 1.4.2 Criar código de benchmarking para código c# - [:heavy_check_mark:]   
---- 1.4.3 Guardar arquivos com resultados de benchmarking - [:heavy_check_mark:]  
-- 1.5 Decisão de soluções a partir de benchmarking e facilidade de generalização - [ ]  

## Agradecimentos  
Agradeço à minha família por sempre me apoiar em escolhas acadêmicas ou profissionais,  
Agradeço aos Professores do curso de Engenharia de Computação da UFC por contribuírem com meu conhecimento acadêmico, em especial ao Prof. Marques Soares por ser orientador do meu TCC,  
Agradeço ao João Lucas de Oliveira Timbó por contribuir com o meu conhecimento e experiência professional,  
Agradeço aos colegas da Universidade por toda ajuda já oferecida diante o curso.
