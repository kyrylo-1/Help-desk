import React, { useState, useContext, useEffect } from 'react';

const Register = props => {
  const [user, setUser] = useState({
    name: '',
    password: '',
    type: 'HelpDeskUser'
  });

  const { name, password, type } = user;

  const onChange = e => setUser({ ...user, [e.target.name]: e.target.value });

  const onSubmit = e => {
    e.preventDefault();
    if (name === '' || password === '') {
      console.error('Please enter all fields');
    } else {
      //   register({
      //     name,
      //     password
      //   });
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
          <label htmlFor="name">Name</label>
          <input
            type="text"
            name="name"
            value={name}
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
