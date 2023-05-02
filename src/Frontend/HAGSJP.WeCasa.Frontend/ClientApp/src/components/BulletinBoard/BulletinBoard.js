import React, { useState, useEffect } from 'react';
import Draggable from 'react-draggable'; // import the Draggable component
import './BulletinBoard.css'; // import the CSS stylesheet
import axios from 'axios';
import { useAuth } from '../AuthContext';

function BulletinBoard() {
  // global variables
  const { auth, currentUser, currentGroup } = useAuth()
  // define state variables for the stickies, pictures, and selected color
  const [stickies, setStickies] = useState([]);
  const [pictures, setPictures] = useState([]);
  const [selectedColor, setSelectedColor] = useState('#ffe8b3');

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
    axios.get(`bulletin-board/${currentGroup.groupId}`).then((response) => { 
      var res = response.data
      setStickies(res)
      console.log("GET SUCCESS res", res)
      console.log("GET SUCCESS with ", currentGroup.groupId, " ", stickies)
    }).catch( (error) => { 
      console.log(error) 
    });
  }

  const getPhotos = () => {
     // Get Pictures
     let groupId = currentGroup['groupId'];
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
          }
         //  else {
         //      failureFileView(res.data['message']);
         //  }
         console.log("Get Pic Success")
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
      groupId: currentGroup.groupId,
      lastModifiedUser: currentUser.username,
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
  
  // function to add a new picture to the board
  // const addPicture = (e, id) => {
  //   const file = e.target.files[0];
  //   const reader = new FileReader();
  //   reader.readAsDataURL(file);
  //   reader.onloadend = () => {
  //   const newPicture = {

  //     id: id,
  //     content: reader.result,
  //     x: Math.floor(Math.random() * (600 - 100) + 100), // generate a random x position for the picture
  //     y: Math.floor(Math.random() * (600 - 100) + 100) // generate a random y position for the picture
  //   };

  //     setPictures([...pictures, newPicture]); // add the new picture to the array of pictures
  //   };
  // };

  const addPicture = (id, e) => {
      const file = e.target.files[0];
    const reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onloadend = () => {
      const newPicture = {

        id: id,
        content: reader.result,
        x: Math.floor(Math.random() * (600 - 100) + 100), // generate a random x position for the picture
        y: Math.floor(Math.random() * (600 - 100) + 100) // generate a random y position for the picture
      };
    };

    const formData = new FormData();
    formData.append('file', file);
    formData.append('name', file.name);
    formData.append('owner', currentUser['username']);
    formData.append('groupId', currentGroup['groupId']);
    
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
            setPictures([...pictures, newPicture]);
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

  // // function to handle adding a new picture to the board
  const handleAddPicture = () => {
    const id = Date.now();
    const input = document.createElement('input');
    input.type = 'file';
    input.accept = 'image/*';
    input.addEventListener('change', (e) => addPicture(e, id));
    input.click();
  };


  return (
    <div className="bulletin-board">
      <div className="color-picker">
        {colors.map(color => (
          <div key={color} className="color-circle" style={{ backgroundColor: color }} onClick={() => setSelectedColor(color)} />
        ))}
      </div>
      <button onClick={addSticky}>Add Sticky</button>
      <button onClick={handleAddPicture}>Add Picture</button>
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
            <img className="picture-img" src={picture.photoFileName} alt="uploaded" />
            <button className="delete-button" onClick={() => handleDeletePicture(picture.id)}>Delete</button>
          </div>
        </Draggable>
      ))}
    </div>
  );
}

export default BulletinBoard;