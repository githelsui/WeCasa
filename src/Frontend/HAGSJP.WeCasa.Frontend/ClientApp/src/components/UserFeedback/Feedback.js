import React, { useState } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button, FormGroup, Label, Input } from 'reactstrap';
import axios from 'axios';

export const Feedback = () => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [email, setEmail] = useState('');
    const [feedbackMessage, setFeedbackMessage] = useState('');
    const [feedbackRating, setRating] = useState(0);
    const [modal, setModal] = useState(false);
    const [feedbackType, setFeedbackType] = useState(null);
    const toggle = () => setModal(!modal);
    const submitFormData = () => {
        let request = {
            submissionDate: new Date().toISOString(),
            feedbackType: feedbackType,
            firstName: firstName,
            lastName: lastName,
            email: email,
            feedbackMessage: feedbackMessage,
            feedbackRating: feedbackRating,
            resolvedStatus: 0,
            resolvedDate: null
        };
        console.log("REQUEST", request)
        const feedbackTypeRadioButtons = document.querySelectorAll('[name="feedbackType"]');

        axios.post('/uploadfeedback', request)
            .then((response) => {
                console.log(response.data); // Handle successful response from server
            })
            .catch((error) => {
                console.error(error); // Handle error response from server
            });
    };

    function handleFeedbackTypeChange(event) {
        const selectedValue = event.target.value;
        if (selectedValue === 'Review') {
            setFeedbackType(1); // Set feedbackType to true for "Review"
        } else if (selectedValue === 'Issue') {
            setFeedbackType(0); // Set feedbackType to false for "Issue"
        } else {
            setFeedbackType(null); // Set feedbackType to null for no selection
        }
    }

    return (
        <div id="feedback-component">
            <p>The development team at WeCasa is dedicated to providing the best experience possible for its users, and we value your feedback!</p>
            <p>By taking the time to leave us a review or report an issue, you are directly impacting the future of our application along with its surrounding community.</p>
            <p>Thank you!</p>
            <Button color="primary" onClick={toggle}>Submit a Feedback Ticket</Button>
            <div>
                <Modal isOpen={modal} toggle={toggle}>
                    <ModalHeader toggle={toggle}>Submit a User Feedback Ticket</ModalHeader>
                    <ModalBody>
                        <FormGroup>
                            <Label>Please specify feedback type:</Label>
                            <div>
                                <Label htmlFor="review">
                                    <input type="radio" name="feedbackType" id="review" value="Review" checked={feedbackType === 1} onChange={handleFeedbackTypeChange} />
                                    {' '}
                                    Review
                                </Label>
                            </div>
                            <div>
                                <Label htmlFor="reportIssue">
                                    <input type="radio" name="feedbackType" id="issue" value="Issue" checked={feedbackType === 0} onChange={handleFeedbackTypeChange} />
                                    {' '}
                                    Report an Issue
                                </Label>
                            </div>
                        </FormGroup>
                        <FormGroup>
                            <Label for="firstName">First Name:</Label>
                            <Input type="text" name="firstName" id="firstName" placeholder="First Name" onChange={e => setFirstName(e.target.value)} />
                        </FormGroup>
                        <FormGroup>
                            <Label for="lastName">Last Name:</Label>
                            <Input type="text" name="lastName" id="lastName" placeholder="Last Name" onChange={e => setLastName(e.target.value)} />
                        </FormGroup>
                        <FormGroup>
                            <Label for="email">Email:</Label>
                            <Input type="email" name="email" id="email" placeholder="Email" onChange={e => setEmail(e.target.value)} />
                        </FormGroup>
                        <FormGroup>
                            <Label for="feedback">User Feedback Message:</Label>
                            <Input type="textarea" name="feedbackMessage" id="feedbackMessage" placeholder="Enter your Feedback Here (200 Character Limit)" onChange={e => setFeedbackMessage(e.target.value)} />
                        </FormGroup>
                        {feedbackType === 1 &&
                            <FormGroup>
                                <Label for="rating">Rating </Label>
                                <input type="range" name="feedbackRating" id="feedbackRating" min="0" max="5" step="0.5" value={feedbackRating} onChange={e => setRating(e.target.value)} />
                                <p>{feedbackRating} stars</p>
                            </FormGroup>
                        }
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" onClick={toggle, submitFormData}>Submit</Button>{''}
                        <Button color="secondary" onClick={toggle}>Cancel</Button>
                    </ModalFooter>
                </Modal>
            </div>
            <p></p>
            <p>For further assistance, email us directly at wecasa@gmail.com</p>
        </div>
    );
};

export default Feedback;
