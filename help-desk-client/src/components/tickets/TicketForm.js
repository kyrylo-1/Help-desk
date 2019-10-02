import React, { useState, useContext, useEffect } from 'react';
import TicketContext from '../../context/ticket/ticketContext';

const TicketForm = () => {
  const ticketContext = useContext(TicketContext);
  const { addTicket } = ticketContext;
  const [description, setDescription] = useState('');

  const onChange = e => setDescription(e.target.value);

  const onSubmit = e => {
    e.preventDefault();
    console.log('onSubmit');
  };

  return (
    <form onSubmit={onSubmit}>
      <h2 className="text-primary">{'Add Ticket'}</h2>
      <input
        type="text"
        placeholder="write description"
        name="description"
        value={description}
        onChange={onChange}
      />
    </form>
  );
};

export default TicketForm;
