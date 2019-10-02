import React, { Fragment, useContext, useEffect } from 'react';
import TicketContext from '../../context/ticket/ticketContext';
import TicketItem from './TicketItem';
import Spinner from '../layout/Spinner';

const Tickets = ({ canDeletTicket }) => {
  const ticketContext = useContext(TicketContext);
  const { tickets, getAllTickets, loading } = ticketContext;

  useEffect(() => {
    getAllTickets();
    // eslint-disable-next-line
  }, []);

  if (tickets !== null && tickets.length === 0 && !loading) {
    return <h4>No tickets</h4>;
  }

  const ticketItems = () => {
    return tickets.map(t => (
      <TicketItem ticket={t} canDelete={canDeletTicket} key={t.id} />
    ));
  };

  return (
    <Fragment>
      {tickets != null && !loading ? ticketItems() : <Spinner />}
    </Fragment>
  );
};

export default Tickets;
