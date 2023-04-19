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

  // Get Stickies
  useEffect(() => {
    axios.get(`bulletin-board/${currentGroup.groupId}`).then((response) => { 
       var res = response.data
       setStickies(res)
       console.log("GET SUCCESS res", res)
     console.log("GET SUCCESS", stickies)
 }).catch( (error) => {
   console.log(error)
  });
  }, []);

  useEffect(() => {
    console.log("Updated stickies:", stickies);
  }, [stickies]);

  // function to add a new sticky note to the board
  const addSticky = () => {
    // const newSticky = {
    //   id: Date.now(),
    //   content: '',
    //   color: selectedColor,
    //   x: Math.floor(Math.random() * (600 - 100) + 100), // generate a random x position for the sticky note
    //   y: Math.floor(Math.random() * (600 - 100) + 100) // generate a random y position for the sticky note
    // };

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
  const addPicture = (e, id) => {
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

      setPictures([...pictures, newPicture]); // add the new picture to the array of pictures
    };
  };
  
  // function to handle deleting a sticky note from the board
  const handleDeleteSticky = (id) => {
    const updatedStickies = stickies.filter(sticky => sticky.noteId !== id);
    setStickies(updatedStickies);
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

  // function to handle adding a new picture to the board
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
            <textarea className="sticky-text" value={sticky.message} onChange={(e) => handleStickyTextChange(sticky.id, e.target.value)} />
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