import React, { Fragment, useContext, useEffect } from 'react';
import AuthContext from '../../context/auth/authContext';
import TicketItem from './TicketItem';

const Tickets = () => {
  const authContext = useContext(AuthContext);
  const { user, loading } = authContext;

  if (user == null) return <Fragment />;

  if (user.tickets !== null && user.tickets.length === 0 && !loading) {
    return <h4>No tickets</h4>;
  }

  const loadingPlaceHolder = <h3>Loading tickets...</h3>;

  return (
    <div>
      {user.tickets.map(t => (
        <TicketItem
          ticket={t}
          canDelete={user.usertype === 'TeamMember'}
          key={t.id}
        />
      ))}
      ;
    </div>
  );
};

export default Tickets;
