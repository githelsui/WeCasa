import React, { useState, useEffect } from 'react';
import { Modal, ModalHeader, ModalBody, ModalFooter, Button, FormGroup, Label, Input, CustomInput } from 'reactstrap';
import { useLocation } from 'react-router-dom';

export const Feedback = () => {
    const location = useLocation();
    const [modal, setModal] = useState(false);

    useEffect(() => {
        if (location.pathname === '/uploadfeedback') {
            setModal(true);
        }
    }, [location.pathname]);

    const toggle = () => setModal(!modal);

    function FeedbackModal(props) {
        const [feedbackType, setFeedbackType] = useState('Report an Issue');

        function handleFeedbackTypeChange(event) {
            setFeedbackType(event.target.value);
        }

        return (
            <Modal isOpen={isOpen} toggle={toggle}>
                <ModalHeader toggle={toggle}>Submit a User Feedback Ticket</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>Please specify feedback type:</label><br />
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="feedbackType" value="review" checked={feedbackType === "review"} onChange={handleFeedbackTypeChange} />
                            <label className="form-check-label">Review</label>
                        </div>
                        <div className="form-check form-check-inline">
                            <input className="form-check-input" type="radio" name="feedbackType" value="reportIssue" checked={feedbackType === "reportIssue"} onChange={handleFeedbackTypeChange} />
                            <label className="form-check-label">Report an Issue</label>
                        </div>
                    </div>
                    <div className="form-group">
                        <label htmlFor="firstName">First Name:</label>
                        <input type="text" className="form-control" id="firstName" value={firstName} onChange={(event) => setFirstName(event.target.value)} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="lastName">Last Name:</label>
                        <input type="text" className="form-control" id="lastName" value={lastName} onChange={(event) => setLastName(event.target.value)} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="email">Email:</label>
                        <input type="email" className="form-control" id="email" value={email} onChange={(event) => setEmail(event.target.value)} />
                    </div>
                    <div className="form-group">
                        <label htmlFor="feedbackMessage">User Feedback Message:</label>
                        <textarea className="form-control" id="feedbackMessage" rows="5" value={feedbackMessage} onChange={(event) => setFeedbackMessage(event.target.value)}></textarea>
                    </div>
                    {feedbackType === "review" && (
                        <div className="form-group">
                            <label htmlFor="rating">Rating:</label>
                            <input type="range" className="form-control-range" id="rating" min="0" max="5" step="0.5" value={rating} onChange={handleRatingChange} />
                            <div className="text-center">{rating}</div>
                        </div>
                    )}
                </ModalBody>
                <ModalFooter>
                    <Button color="primary" onClick={toggle}>Submit</Button>{' '}
                    <Button color="secondary" onClick={toggle}>Cancel</Button>
                </ModalFooter>
            </Modal>
        );
    }
};

export default Feedback;