import React, { useState, useEffect } from 'react';
import { Button } from 'antd';
import { PlusCircleOutlined, FileOutlined } from '@ant-design/icons'
import Draggable from 'react-draggable'; // import the Draggable component
import './BulletinBoard.css'; // import the CSS stylesheet
import axios from 'axios';
import { useAuth } from '../Auth/AuthContext';
import config from '../../appsettings.json';
import * as Styles from '../../styles/ConstStyles.js';
import { UploadOutlined } from '@ant-design/icons';



function BulletinBoard() {
  // global variables
  // const { auth, currentUser, currentGroup } = useAuth()
  // define state variables for the stickies, pictures, and selected color
  const [stickies, setStickies] = useState([]);
  const [pictures, setPictures] = useState([]);
  const [selectedColor, setSelectedColor] = useState('#ffe8b3');
  const fileInputRef = React.createRef();

  const maxFileSize = 10 * 1024 * 1024; // 10 MB
  const maxFilesPerPage = config.maxFilesPerPage;
  const validFileTypes = config.validFileTypes;
  const validFileExt = config.validFileExt;

  const GID = 1235529
  const user = "joy@gmail.com"

  // define an array of colors to use for the color picker
  const colors = ['#ffe8b3', '#b3ffe8', '#ffb3e8', '#e8b3ff'];

  const getBlobType = (fileType) => {
    var blobType = '';
    switch (fileType) {
        case '.jpg' || '.jpeg' || '.png' || '.gif':
            blobType = `image/${fileType.slice(1)}`;
            break;
        case '.txt':
            blobType = 'text/plain';
            break;
        case '.html':
            blobType = 'text/html';
            break;
        case '.pdf':
            blobType = 'application/pdf';
            break;
        case '.doc':
            blobType = 'application/msword';
            break;
        case '.docx':
            blobType = 'application/vnd.openxmlformats-officedocument.wordprocessingml.document';
            break;
    }
    return blobType;
  }

  // Get Stickies
  useEffect(() => {
    getSticky()
    getPhotos()
  }, []);

  const getSticky = () => {
    // Get Sticky notes
    axios.get(`bulletin-board/${GID}`).then((response) => { 
      var res = response.data
      setStickies(res)
      console.log("GET SUCCESS res", res)
      console.log("GET SUCCESS with ", GID, " ", stickies)
    }).catch( (error) => { 
      console.log(error) 
    });
  }

  const getPhotos = () => {
     // Get Pictures
     let groupId = GID;
     axios.get('files/GetGroupFiles', { params: { groupId }})
       .then(res => {
          var isSuccessful = res.data['isSuccessful']
          if (isSuccessful) {
               var fileContents = []
               fileContents = res.data['returnedObject'].map(file => {
                   // decoding the base-64 string data to binary array
                   const binaryData = atob(file['data']);
                   // creating an array buffer to perform data manipulation on the binary data
                   const arrayBuffer = new ArrayBuffer(binaryData.length);
                   // creating an array of 8-bit unsigned integers necessary for creating the Blob
                   const uint8Array = new Uint8Array(arrayBuffer);
                   // converting binary data into string representation
                   for (let i = 0; i < binaryData.length; i++) {
                       uint8Array[i] = binaryData.charCodeAt(i);
                   }
                   const blobType = getBlobType(file.contentType);
                   const blob = new Blob([uint8Array], { type: blobType })
                   console.log("PHOTO SUCCESS ", binaryData)
                   return {
                       ...file,
                       owner: file.fileName.split('/').slice(0, -1).join('/'),
                       fileName: file.fileName.split('/').pop(),
                       data: binaryData,
                       blobType: blobType,
                       url: URL.createObjectURL(blob)
                   }
               });
               setPictures(fileContents);
               console.log("Get Pic Success file content ", fileContents)
          }
         //  else {
         //      failureFileView(res.data['message']);
         //  }
       })
       .catch((error) => {
           console.error(error)
       });
  }

  useEffect(() => {
    console.log("Updated stickies:", stickies);
  }, [stickies]);

  // function to add a new sticky note to the board
  const addSticky = () => {
    const newSticky = {
      message: "",
      groupId: GID,
      lastModifiedUser: user,
      color: selectedColor,
      x: Math.floor(Math.random() * (600 - 100) + 100), // generate a random x position for the sticky note
      y: Math.floor(Math.random() * (600 - 100) + 100) // generate a random y position for the sticky note
    }

    axios.post('bulletin-board/Add', newSticky).then(res => {
      var response = res.data;
      console.log(response);
    }).catch((error => { console.error(error) }));
    setStickies([...stickies, newSticky]); // add the new sticky note to the array of stickies
  };

  const handleFileInputChange = (event) => {
    const file = event.target.files[0];
    if (!validFileTypes.includes(file.type)) {
        // setValidInput(false);
        // toast('Invalid file type');
        return;
    }
    if (!validFileExt.includes(file.name.split(".").pop())) {
        console.log(file.name.split(".").pop());
        // setValidInput(false);
        // toast('Invalid file type');
        return;
    }
    if (file.size > maxFileSize) {
        // setValidInput(false);
        // toast('File is too large');
        return;
    }
    // setValidInput(true);

    setPictures(file);
    addPicture(file);
}

  const addPicture = (file) => {
    const formData = new FormData();
    formData.append('file', file);
    formData.append('name', file.name);
    formData.append('owner', user);
    formData.append('groupId', GID);
    console.log("FILE NAME", file.name)
    for (const [key, value] of formData.entries()) {
      console.log(`${key}: ${value}`);
    }
    
    axios.post('files/UploadFile', formData, { headers: { 'Content-Type': 'multipart/form-data' }})
        .then(res => {
            var isSuccessful = res.data['isSuccessful'];
            if (isSuccessful) {
                // successFileView(res.data['message']);
                getPhotos();
            }
            // else {
                // failureFileView(res.data['message']);
            // }
        })
        .catch((error) => {
            console.error(error);
        });
}
  
  // function to handle deleting a sticky note from the board
  const handleDeleteSticky = (id) => { 
    const updatedStickies = stickies.filter(sticky => sticky.noteId !== id);
    console.log('ID ', id) 
    axios.delete(`bulletin-board/Delete/${id}`).then((response) => { 
      let res = response
      if (res) {
        console.log('Delete Successful')
        setStickies(updatedStickies);
      } else {
        console.log('Delete Failed') 
      }
    }).catch(() => { alert('Delete Failed') }); 
  };

  // function to handle deleting a picture from the board
  const handleDeletePicture = (id) => {
    const updatedPictures = pictures.filter(picture => picture.id !== id);
    setPictures(updatedPictures);
  };

  // function to handle changing the text of a sticky note
  const handleStickyTextChange = (id, value) => {
    const updatedStickies = stickies.map(sticky => {
      if (sticky.noteId === id) {
        return { ...sticky, message: value };
      }
      return sticky;
    });
    setStickies(updatedStickies);
  };

  // function to handle changing updating the text in the backend
  const handleStickyTextBlur = (id, value) => {
    let requestSticky;
    const updatedStickies = stickies.map(sticky => {
      if (sticky.noteId === id) {
        requestSticky = sticky;
        return { ...sticky, message: value };
      }
      return sticky;
    });

    axios.post('bulletin-board/Update', requestSticky).then(res => {
      if (res) {
        console.log('Update Successful')
        setStickies(updatedStickies);
      } else {
        console.log('Update Failed') 
      }
    })
    .catch((error => { console.error(error) }));
  };

  const getUserFile = () => {
      fileInputRef.current.click();
  }


  return (
    <div className="bulletin-board" style={{ display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
      <div className="color-picker">
        {colors.map(color => (
          <div key={color} className="color-circle" style={{ backgroundColor: color }} onClick={() => setSelectedColor(color)} />
        ))}
      </div>
      <Button
          id="add-sticky"
          style={Styles.addButtonStyle}
          shape="round"
          icon={<PlusCircleOutlined />}
          size={'large'}
          onClick={() => addSticky()}>
          Add Sticky
      </Button>
      <Button
          id="add-photo"
          style={Styles.addButtonStyle}
          shape="round"
          icon={<UploadOutlined/>}
          size={'large'}
          onClick={() => getUserFile()}>
          Add Photo
      </Button>
      {stickies.map(sticky => (
        <Draggable key={sticky.id} defaultPosition={{x: sticky.x, y: sticky.y}}>
          <div className="sticky-note" style={{ backgroundColor: sticky.color }}>
          <textarea className="sticky-text" value={sticky.message} onChange={(e) => handleStickyTextChange(sticky.noteId, e.target.value)} onBlur={(e) => handleStickyTextBlur(sticky.noteId, e.target.value)}/>
            <button className="delete-button" onClick={() => handleDeleteSticky(sticky.noteId)}>Delete</button>
          </div>
        </Draggable>
      ))}
      {pictures.map(picture => (
        <Draggable key={picture.id} defaultPosition={{x: picture.x, y: picture.y}}>
          <div className="picture">
          <input type="file" ref={fileInputRef} onChange={handleFileInputChange} style={{ display: 'none'}}></input>

          {/* <input type="file" ref={fileInputRef} onChange={(event) => handleFileInputChange(event)} style={{ display: 'none'}}></input> */}
            {/* <img className="picture-img" src={picture.photoFileName} alt="uploaded" /> */}
            <button className="delete-button" onClick={() => handleDeletePicture(picture.id)}>Delete</button>
          </div>
        </Draggable>
      ))}
    </div>
  );
}

export default BulletinBoard;