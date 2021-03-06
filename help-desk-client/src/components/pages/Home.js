import React, { Fragment, useContext, useEffect } from 'react';
import Tickets from '../tickets/Tickets';
import TicketForm from '../tickets/TicketForm';
import AuthContext from '../../context/auth/authContext';

const Home = () => {
  const authContext = useContext(AuthContext);
  const { user, loading } = authContext;
  // debugger;
  useEffect(() => {
    authContext.loadUser();
    // eslint-disable-next-line
  }, []);

  const loadingPart = (
    <div>
      <h1>Loading...</h1>
    </div>
  );

  return (
    <Fragment>
      {loading ? (
        loadingPart
      ) : (
        <div>
          <h1>You loged as {user && user.type}</h1>
          {user && (
            <div className="grid-2">
              <div>
                <TicketForm canAdd={user.type === 'HelpDeskUser'} />
              </div>
              <div>
                <Tickets canDeletTicket={user.type === 'TeamMember'} />
              </div>
            </div>
          )}
        </div>
      )}
    </Fragment>
  );
};

export default Home;
