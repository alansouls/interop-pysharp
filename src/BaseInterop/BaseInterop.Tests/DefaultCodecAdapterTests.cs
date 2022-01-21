using BaseInterop.DataTransfer;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace BaseInterop.Tests
{
    public class DefaultCodecAdapterTests
    {
        private readonly DefaultCodecAdapter _adapter;

        public DefaultCodecAdapterTests()
        {
            _adapter = new DefaultCodecAdapter();
        }

        [Fact]
        public void ShouldEncodeIntAccordingToContract()
        {
            int integer = 1;
            var bytes = _adapter.GetBytes(integer);

            bytes.Length.Should().Be(6);
            bytes[0].Should().Be(0); //representing the buffer contains simple typed data
            bytes[1].Should().Be(0); //representing that the buffer data type is int
            bytes[2..6].Should().BeEquivalentTo(new[] { 1, 0, 0, 0 }); //representing the buffer data
        }

        [Fact]
        public void ShouldEncodeDoubleAccordingToContract()
        {
            double floatNum = 1.0f;
            var bytes = _adapter.GetBytes(floatNum);

            bytes.Length.Should().Be(10);
            bytes[0].Should().Be(0); //representing the buffer contains simple typed data
            bytes[1].Should().Be(1); //representing that the buffer data type is float
            bytes[2..10].Should().BeEquivalentTo(new[] { 0, 0, 0, 0, 0, 0, 0xF0, 0x3F }); //representing the buffer data
        }

        [Fact]
        public void ShouldEncodeStringAccordingToContract()
        {
            string str = "ufc";
            var bytes = _adapter.GetBytes(str);

            bytes.Length.Should().Be(9);
            bytes[0].Should().Be(1); //representing the buffer contains array typed data
            bytes[1].Should().Be(2); //representing that the buffer data type is char
            bytes[2..6].Should().BeEquivalentTo(new[] { 3, 0, 0, 0 }); //representing the size of the data as an int
            bytes[6..9].Should().BeEquivalentTo(new[] { 0x75, 0x66, 0x63 }); //representing the buffer data "ufc"
        }

        [Fact]
        public void ShouldEncodeBoolAccordingToContract()
        {
            bool boolean = true;
            var bytes = _adapter.GetBytes(boolean);

            bytes.Length.Should().Be(3);
            bytes[0].Should().Be(0); //representing the buffer contains simple typed data
            bytes[1].Should().Be(3); //representing that the buffer data type is bool
            bytes[2].Should().Be(1); //representing the buffer data on little endianess
        }

        [Fact]
        public void ShouldEncodeIntArrayAccordingToContract()
        {
            int[] integers = new[] { 1, 2 };
            var bytes = _adapter.GetBytes(integers);

            bytes.Length.Should().Be(14);
            bytes[0].Should().Be(1); //representing the buffer contains array typed data
            bytes[1].Should().Be(0); //representing that the buffer data type is int
            bytes[2..6].Should().BeEquivalentTo(new[] { 2, 0, 0, 0 }); //representing the size of the data as an int
            bytes[6..10].Should().BeEquivalentTo(new[] { 1, 0, 0, 0 }); //representing the buffer data on little endianess
            bytes[10..14].Should().BeEquivalentTo(new[] { 2, 0, 0, 0 }); //representing the buffer data on little endianess
        }

        [Fact]
        public void ShouldEncodeAnyIntEnumerableAsAnIntArrayAccordingToContract()
        {
            int[] integers = new[] { 1, 2 };
            var bytes = _adapter.GetBytes(integers);

            IEnumerable<int> enumerable = new List<int>() { 1, 2 };
            var enumerableBytes = _adapter.GetBytes(enumerable);

            bytes.Should().BeEquivalentTo(enumerableBytes);
        }

        [Fact]
        public void ShouldEncodeDoubleArrayAccordingToContract()
        {
            double[] integers = new[] { 1.0, 2.0 };
            var bytes = _adapter.GetBytes(integers);

            bytes.Length.Should().Be(22);
            bytes[0].Should().Be(1); //representing the buffer contains array typed data
            bytes[1].Should().Be(1); //representing that the buffer data type is double
            bytes[2..6].Should().BeEquivalentTo(new[] { 2, 0, 0, 0 }); //representing the size of the data as an int
            bytes[6..14].Should().BeEquivalentTo(new[] { 0, 0, 0, 0, 0, 0, 0xF0, 0x3F }); //representing the buffer data
            bytes[14..22].Should().BeEquivalentTo(new[] { 0, 0, 0, 0, 0, 0, 0, 0x40 }); //representing the buffer data
        }

        [Fact]
        public void ShouldEncodeAnyDoubleEnumerableAsADoubleArrayAccordingToContract()
        {
            double[] integers = new[] { 1.0, 2.0 };
            var bytes = _adapter.GetBytes(integers);
            IEnumerable<double> enumerable = new List<double>() { 1.0, 2.0 };
            var enumerableBytes = _adapter.GetBytes(enumerable);
            bytes.Should().BeEquivalentTo(enumerableBytes);
        }

        [Fact]
        public void ShouldDecodeIntDataAccordingToContract()
        {
            byte[] buffer = new byte[] { 0, 0, 1, 0, 0, 0 };

            var result = _adapter.ReadDataFromBytes<int>(buffer);

            result.Should().Be(1);
        }

        [Fact]
        public void ShouldDecodeDoubleDataAccordingToContract()
        {
            byte[] buffer = new byte[] { 0, 1, 0, 0, 0, 0, 0, 0, 0xF0, 0x3F };

            var result = _adapter.ReadDataFromBytes<double>(buffer);

            result.Should().Be(1.0);
        }

        [Fact]
        public void ShouldDecodeStringDataAccordingToContract()
        {
            byte[] buffer = new byte[] { 1, 2, 3, 0, 0, 0, 0x75, 0x66, 0x63 };

            var result = _adapter.ReadDataFromBytes<string>(buffer);

            result.Should().Be("ufc");
        }

        [Fact]
        public void ShouldDecodeBoolDataAccordingToContract()
        {
            byte[] buffer = new byte[] { 0, 3, 1 };

            var result = _adapter.ReadDataFromBytes<bool>(buffer);

            result.Should().Be(true);
        }

        [Fact]
        public void ShouldDecodeIntArrayDataAccordingToContract()
        {
            byte[] buffer = new byte[] { 1, 0, 2, 0, 0, 0, 1, 0, 0, 0, 2, 0, 0, 0 };

            var result = _adapter.ReadDataFromBytes<int[]>(buffer);

            result.Should().BeEquivalentTo(new[] { 1, 2});
        }

        [Fact]
        public void ShouldDecodeDoubleArrayDataAccordingToContract()
        {
            byte[] buffer = new byte[] { 1, 1, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0xF0, 0x3F, 0, 0, 0, 0, 0, 0, 0, 0x40 };

            var result = _adapter.ReadDataFromBytes<double[]>(buffer);

            result.Should().BeEquivalentTo(new[] { 1.0, 2.0 });
        }
    }
}
