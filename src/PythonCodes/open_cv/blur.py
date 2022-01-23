import cv2 as cv
import numpy as np
import os

def blur(src_img, intensity):
    img = cv.imread(src_img)
    blur = cv.blur(img, (intensity, intensity))
    filename = os.path.basename(src_img)
    folderName = os.path.dirname(src_img)
    out_img = os.path.join(folderName, 'blur_' + filename)
    cv.imwrite(out_img, blur)
    return out_img


if __name__ == '__main__':
    blur('test_images/cards.jpg', 10)