import numpy as np
import cv2 as cv
import os

def contour(src_img):
    img = cv.imread(src_img)
    gray = cv.cvtColor(img, cv.COLOR_BGR2GRAY)
    ret, thresh = cv.threshold(gray, 127, 255, 0)
    contours, hierarchy = cv.findContours(thresh, cv.RETR_TREE, cv.CHAIN_APPROX_SIMPLE)
    cv.drawContours(img, contours, -1, (0,255,0), 3)
    filename = os.path.basename(src_img)
    folderName = os.path.dirname(src_img)
    out_img = os.path.join(folderName, 'contour_' + filename)
    cv.imwrite(out_img, img)

    return out_img


if __name__ == '__main__':
    contour('test_images/cards.jpg')