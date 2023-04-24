import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button, FormGroup, Label, Input} from 'reactstrap';
import { useLocation } from 'react-router-dom';
import axios from 'axios';

export const Feedback = () => {
    const location = useLocation();
    const [modal, setModal] = useState(false);
    const [feedbackType, setFeedbackType] = useState('Report an Issue');

    function handleFeedbackTypeChange(event) {
        setFeedbackType(event.target.value);
    }

    useEffect(() => {
        if (location.pathname === '/uploadfeedback') {
            setModal(true);
        }
    }, [location.pathname]);

    const toggle = () => setModal(!modal);

    return (
        <div>
            <Modal isOpen={modal} toggle={toggle}>
                <ModalHeader toggle={toggle}>Submit a User Feedback Ticket</ModalHeader>
                <ModalBody>
                    <FormGroup>
                        <Label>Please specify feedback type:</Label>
                        <div>
                            <Label htmlFor="review">
                                <input type="radio" name="feedbackType" id="review" value="Review" checked={feedbackType === 'Review'} onChange={handleFeedbackTypeChange} />
                                {' '}
                                Review
                            </Label>
                        </div>
                        <div>
                            <Label htmlFor="reportIssue">
                                <input type="radio" name="feedbackType" id="reportIssue" value="Report an Issue" checked={feedbackType === 'Report an Issue'} onChange={handleFeedbackTypeChange} />
                                {' '}
                                Report an Issue
                            </Label>
                        </div>
                    </FormGroup>
                    <FormGroup>
                        <Label for="firstName">First Name:</Label>
                        <Input type="text" name="firstName" id="firstName" placeholder="First Name" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="lastName">Last Name:</Label>
                        <Input type="text" name="lastName" id="lastName" placeholder="Last Name" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="email">Email:</Label>
                        <Input type="email" name="email" id="email" placeholder="Email" />
                    </FormGroup>
                    <FormGroup>
                        <Label for="feedback">User Feedback Message:</Label>
                        <Input type="textarea" name="feedback" id="feedback" placeholder="Enter your feedback here" />
                    </FormGroup>
                    {feedbackType === 'Review' &&
                        <FormGroup>
                            <Label for="rating">Rating</Label>
                            <Input type="select" name="rating" id="rating">
                                <option value="0.5">0.5 stars</option>
                                <option value="0.5">0.5 stars</option>
                                <option value="1.5">1.5 stars</option>
                                <option value="2">2 stars</option>
                                <option value="2.5">2.5 stars</option>
                                <option value="3">3 stars</option>
                                <option value="3.5">3.5 stars</option>
                                <option value="4">4 stars</option>
                                <option value="4.5">4.5 stars</option>
                                <option value="5">5 stars</option>
                            </Input>
                        </FormGroup>
                    }
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={toggle}>Submit</Button>{' '}
                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                </ModalFooter>
            </Modal>
        </div>
    );
};

export default Feedback;
