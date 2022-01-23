import face_recognition

def check_facial_validation(ref_pic, target_pic):   # check if the facial recognition is valid
    # load the known image
    known_image = face_recognition.load_image_file(ref_pic)
    # get the face encodings for the known image
    known_face_encoding = face_recognition.face_encodings(known_image)[0]
    # load the unknown image    
    unknown_image = face_recognition.load_image_file(target_pic)
    # get the face encodings for the unknown image
    unknown_face_encoding = face_recognition.face_encodings(unknown_image)[0]
    # compare the faces
    results = face_recognition.compare_faces([known_face_encoding], unknown_face_encoding)
    # check if the faces are the same
    return bool(results[0])
