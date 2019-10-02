import React, { useContext } from 'react';
import PropTypes from 'prop-types';

const TicketItem = () => {
  return (
    <div className="card bg-light">
      <h3 className="text-primary text-left">Some text on card</h3>
      <p>
        <button className="btn btn-dark btn-sm">Edit</button>
        <button className="btn btn-danger btn-sm">Delete</button>
      </p>
    </div>
  );
};

export default TicketItem;
