import time
import interop_opencv

start = time.time()
interop_opencv.blur('test_images/cards.jpg', 10)
end = time.time()
print((end - start) * 1000)

start = time.time()
interop_opencv.contour('test_images/cards.jpg')
end = time.time()
print((end - start) * 1000)

start = time.time()
interop_opencv.segmentation('test_images/cards.jpg')
end = time.time()
print((end - start) * 1000)
