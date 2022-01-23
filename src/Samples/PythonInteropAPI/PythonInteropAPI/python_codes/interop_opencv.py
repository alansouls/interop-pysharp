import blur  as b
import contour as c
import segmentation as s

def blur(src_img, intensity):
    return b.blur(src_img, intensity)

def contour(src_img):
    return c.contour(src_img)

def segmentation(src_img):
    return s.segmentation(src_img)