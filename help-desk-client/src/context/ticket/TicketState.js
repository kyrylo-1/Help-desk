import React, { useReducer } from 'react';
import axios from 'axios';
import TicketContext from './ticketContext';
import ticketReducer from './ticketReducer';
import setAuthToken from '../../utils/setAuthToken';
import {
  GET_ALL_TICKETS,
  DELETE_TICKET,
  ADD_TICKET,
  UPDATE_TICKET,
  CLEAR_CURRENT_TICKET,
  SET_CURRENT_TICKET,
  TICKET_ERROR
} from '../types';

const TicketState = props => {
  const initialState = {
    tickets: null,
    current: null,
    error: null
  };

  const [state, dispatch] = useReducer(ticketReducer, initialState);

  const getAllTickets = async () => {
    try {
      const res = await axios.get('/api/user/tickets', {
        baseURL: 'http://localhost:5000'
      });

      dispatch({
        type: GET_ALL_TICKETS,
        payload: res.data
      });
    } catch (err) {
      dispatch({ type: TICKET_ERROR });
    }
  };

  return (
    <TicketContext.Provider
      value={{
        tickets: state.tickets,
        current: state.current,
        error: state.error,
        getAllTickets
      }}
    >
      {props.children}
    </TicketContext.Provider>
  );
};

export default TicketState;
