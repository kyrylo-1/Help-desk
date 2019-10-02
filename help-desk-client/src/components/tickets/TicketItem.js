import React, { useContext } from 'react';
import PropTypes from 'prop-types';

const TicketItem = ({ ticket, canDelete }) => {
  const { description, dateAdded } = ticket;

  return (
    <div className="card bg-light">
      <h3 className="text-primary text-left">{description}</h3>
      <h3 className="text-primary text-right">{dateAdded}</h3>
      <p>
        <button className="btn btn-dark btn-sm">Edit</button>
        {canDelete && <button className="btn btn-danger btn-sm">Delete</button>}
      </p>
    </div>
  );
};

export default TicketItem;
