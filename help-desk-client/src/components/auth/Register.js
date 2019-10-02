import React, { useState, useContext, useEffect } from 'react';
import AuthContext from '../../context/auth/authContext';

const Register = props => {
  const authContext = useContext(AuthContext);

  const { register, login, error, clearErrors, isAuthenticated } = authContext;

  useEffect(() => {
    if (isAuthenticated) {
      console.log('Redirect to home page');
      props.history.push('/');
    }
  }, [error, isAuthenticated, props.history]);

  const [user, setUser] = useState({
    username: '',
    password: '',
    type: 'HelpDeskUser'
  });

  const { username, password, type } = user;

  const onChange = e => setUser({ ...user, [e.target.name]: e.target.value });

  const onSubmit = e => {
    e.preventDefault();
    if (username === '' || password === '') {
      console.error('Please enter all fields');
    } else {
      register({
        username,
        password,
        type
      });
      console.log('Register');
    }
  };

  return (
    <div className="form-container">
      <h1>
        User <span className="text-primary">Register</span>
      </h1>
      <form onSubmit={onSubmit}>
        <div className="form-group">
          <label htmlFor="username">Name</label>
          <input
            type="text"
            name="username"
            value={username}
            onChange={onChange}
            required
          />
        </div>
        <div className="form-group">
          <label htmlFor="password">Password</label>
          <input
            type="password"
            name="password"
            value={password}
            onChange={onChange}
            required
            minLength="6"
            maxLength="32"
          />
        </div>
        <input
          type="radio"
          name="type"
          value="HelpDeskUser"
          checked={type === 'HelpDeskUser'}
          onChange={onChange}
        />{' '}
        HelpDesk{' '}
        <input
          type="radio"
          name="type"
          value="TeamMember"
          checked={type === 'TeamMember'}
          onChange={onChange}
        />
        {'  '}
        TeamMember
        <input
          type="submit"
          value="Register"
          className="btn btn-primary btn-block"
        />
      </form>
    </div>
  );
};

export default Register;
