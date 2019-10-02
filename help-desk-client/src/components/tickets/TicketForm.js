import React, { useState, useContext, useEffect } from 'react';
import TicketContext from '../../context/ticket/ticketContext';

const TicketForm = () => {
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

  return (
    <form onSubmit={onSubmit}>
      <h2 className="text-primary">{current ? 'Edit Ticket' : 'Add Ticket'}</h2>
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
          value={current ? 'Edit' : 'Add'}
          className="btn btn-primary btn-block"
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

export default TicketForm;
