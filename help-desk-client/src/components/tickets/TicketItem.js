import React, { useContext } from 'react';
import TicketContext from '../../context/ticket/ticketContext';
import PropTypes from 'prop-types';

const TicketItem = ({ ticket, canDelete }) => {
  const ticketContext = useContext(TicketContext);
  const { setCurrent, clearCurrent, deleteTicket } = ticketContext;

  const { id, description, dateAdded } = ticket;

  const onDelete = () => {
    deleteTicket(id);
    clearCurrent();
  };

  return (
    <div className="card bg-light">
      <h3 className="text-primary text-left">{description}</h3>
      <h3 className="text-primary text-right">{dateAdded}</h3>
      <p>
        <button
          className="btn btn-dark btn-sm"
          onClick={() => setCurrent(ticket)}
        >
          Edit
        </button>
        {canDelete && (
          <button className="btn btn-danger btn-sm" onClick={onDelete}>
            Delete
          </button>
        )}
      </p>
    </div>
  );
};

TicketItem.propTypes = {
  ticket: PropTypes.object.isRequired,
  canDelete: PropTypes.bool.isRequired
};

export default TicketItem;
