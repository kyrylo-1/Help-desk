import React, { useState, useContext, useEffect } from 'react';
import TicketContext from '../../context/ticket/ticketContext';
import PropTypes from 'prop-types';

const TicketForm = ({ canAdd }) => {
  const ticketContext = useContext(TicketContext);
  const { addTicket, current, clearCurrent, updateTicket } = ticketContext;

  useEffect(() => {
    if (current != null) {
      setDescription({
        description: current.description
      });
    } else {
      setDescription({
        description: ''
      });
    }
  }, [current]);

  const [ticket, setDescription] = useState({
    description: ''
  });

  const { description } = ticket;

  const onChange = e =>
    setDescription({ ...ticket, [e.target.name]: e.target.value });

  const onSubmit = e => {
    e.preventDefault();
    console.log('onSubmit');
    if (current === null) {
      addTicket(ticket);
    } else {
      updateTicket({ id: current.id, description: description });
    }
    clearAll();
  };

  const clearAll = () => {
    clearCurrent();
  };

  const showActionText = () => {
    if (canAdd) {
      return current ? 'Edit Ticket' : 'Add Ticket';
    } else {
      return 'Edit Ticket';
    }
  };

  const showInputText = () => {
    if (canAdd) {
      return current ? 'Edit' : 'Add';
    } else {
      return 'Edit';
    }
  };

  const editAddBtnClass = () => {
    return !canAdd && !current
      ? 'btn btn-primary btn-block btn-light'
      : 'btn btn-primary btn-block';
  };

  return (
    <form onSubmit={onSubmit}>
      <h2 className="text-primary">{showActionText()}</h2>
      <input
        type="text"
        placeholder="write description"
        name="description"
        value={description}
        onChange={onChange}
        required
      />
      <div>
        <input
          type="submit"
          value={showInputText()}
          className={editAddBtnClass()}
        />
      </div>
      {current && (
        <div>
          <button className="btn btn-light btn-block" onClick={clearAll}>
            Clear
          </button>
        </div>
      )}
    </form>
  );
};

TicketForm.propTypes = {
  canAdd: PropTypes.bool.isRequired
};

export default TicketForm;
