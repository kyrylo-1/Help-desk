import React, { Fragment, useContext, useEffect } from 'react';
import Tickets from '../tickets/Tickets';
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
          {user && <Tickets canDeletTicket={user.type === 'TeamMember'} />}
        </div>
      )}
    </Fragment>
  );
};

export default Home;
