import React, { useReducer } from 'react';
import axios from 'axios';
import AuthContext from './authContext';
import authReducer from './authReducer';
import setAuthToken from '../../utils/setAuthToken';
import {
  REGISTER_SUCCESS,
  REGISTER_FAIL,
  USER_LOADED,
  AUTH_ERROR,
  LOGIN_SUCCESS,
  LOGIN_FAIL,
  LOGOUT,
  CLEAR_ERRORS
} from '../types';

const AuthState = props => {
  const initialState = {
    token: localStorage.getItem('token'),
    isAuthenticated: null,
    loading: true,
    user: null,
    error: null
  };
  const configForRequest = {
    baseURL: 'http://localhost:5000',
    headers: {
      'Content-Type': 'application/json'
    }
  };

  const [state, dispatch] = useReducer(authReducer, initialState);

  const loadUser = async () => {
    // debugger;
    console.log('loadUser');
    if (localStorage.token) {
      setAuthToken(localStorage.token);
    }
    try {
      const res = await axios.get('/api/user', {
        baseURL: 'http://localhost:5000'
      });

      dispatch({
        type: USER_LOADED,
        payload: res.data
      });
    } catch (err) {
      dispatch({ type: AUTH_ERROR });
    }
  };

  const register = async formData => {
    try {
      const res = await axios.post(
        '/api/auth/register',
        formData,
        configForRequest
      );

      dispatch({
        type: REGISTER_SUCCESS,
        payload: res.data
      });
      loadUser();
    } catch (err) {
      dispatch({
        type: REGISTER_FAIL,
        payload: err.response.data.msg
      });
    }
  };

  const login = async formData => {
    try {
      const res = await axios.post(
        '/api/auth/login',
        formData,
        configForRequest
      );

      dispatch({
        type: LOGIN_SUCCESS,
        payload: res.data
      });

      // debugger;
      loadUser();
    } catch (err) {
      dispatch({
        type: LOGIN_FAIL,
        payload: err.response.data.msg
      });
    }
  };

  const logout = () => dispatch({ type: LOGOUT });

  const clearErrors = () => dispatch({ type: CLEAR_ERRORS });

  return (
    <AuthContext.Provider
      value={{
        token: state.token,
        isAuthenticated: state.isAuthenticated,
        loading: state.loading,
        user: state.user,
        error: state.error,
        register,
        login,
        loadUser,
        logout,
        clearErrors
      }}
    >
      {props.children}
    </AuthContext.Provider>
  );
};

export default AuthState;
