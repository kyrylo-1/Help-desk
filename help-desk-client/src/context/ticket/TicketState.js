import React, { useReducer } from 'react';
import axios from 'axios';
import TicketContext from './ticketContext';
import ticketReducer from './ticketReducer';
import { config } from '../../config/config';
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
  const baseURL = config.url.API_URL;

  const getAllTickets = async () => {
    try {
      const res = await axios.get('/api/user/tickets', {
        baseURL: baseURL
      });

      dispatch({
        type: GET_ALL_TICKETS,
        payload: res.data
      });
    } catch (err) {
      dispatch({ type: TICKET_ERROR });
    }
  };

  const addTicket = async ticketData => {
    try {
      const res = await axios.post('/api/user/tickets/', ticketData, {
        baseURL: baseURL
      });

      dispatch({
        type: ADD_TICKET,
        payload: res.data
      });
    } catch (err) {
      dispatch({ type: TICKET_ERROR });
    }
  };

  const deleteTicket = async id => {
    try {
      await axios.delete(`/api/user/tickets/${id}`, {
        baseURL: baseURL
      });

      dispatch({
        type: DELETE_TICKET,
        payload: id
      });
    } catch (err) {
      dispatch({ type: TICKET_ERROR });
    }
  };

  const updateTicket = async ticket => {
    try {
      const res = await axios.patch(
        `/api/user/tickets/${ticket.id}`,
        {
          description: ticket.description
        },
        {
          baseURL: baseURL
        }
      );

      dispatch({
        type: UPDATE_TICKET,
        payload: res.data
      });
    } catch (err) {
      dispatch({ type: TICKET_ERROR });
    }
  };

  const setCurrent = ticket => {
    dispatch({ type: SET_CURRENT_TICKET, payload: ticket });
  };

  const clearCurrent = () => {
    dispatch({ type: CLEAR_CURRENT_TICKET });
  };

  return (
    <TicketContext.Provider
      value={{
        tickets: state.tickets,
        current: state.current,
        error: state.error,
        getAllTickets,
        addTicket,
        setCurrent,
        clearCurrent,
        updateTicket,
        deleteTicket
      }}
    >
      {props.children}
    </TicketContext.Provider>
  );
};

export default TicketState;
