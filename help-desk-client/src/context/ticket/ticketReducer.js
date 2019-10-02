import {
  GET_ALL_TICKETS,
  DELETE_TICKET,
  ADD_TICKET,
  UPDATE_TICKET,
  CLEAR_CURRENT_TICKET,
  SET_CURRENT_TICKET,
  TICKET_ERROR
} from '../types';

export default (state, action) => {
  switch (action.type) {
    case GET_ALL_TICKETS:
      return {
        ...state,
        tickets: action.payload,
        loading: false
      };
    case ADD_TICKET:
      return {
        ...state,
        tickets: [action.payload, ...state.tickets],
        loading: false
      };
    case UPDATE_TICKET:
      return {
        ...state,
        tickets: state.tickets.map(ticket =>
          ticket.id === action.payload.id ? action.payload : ticket
        ),
        loading: false
      };
    case DELETE_TICKET:
      return {
        ...state,
        tickets: state.tickets.filter(ticket => ticket.id !== action.payload),
        loading: false
      };
    case CLEAR_CURRENT_TICKET:
      return {
        ...state,
        current: null
      };
    case SET_CURRENT_TICKET:
      return {
        ...state,
        current: action.payload
      };
    case TICKET_ERROR:
      return {
        ...state,
        error: action.payload
      };
    default:
      return state;
  }
};
