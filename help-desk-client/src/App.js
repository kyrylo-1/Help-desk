import React from 'react';
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import './App.css';
import Register from './components/auth/Register';

function App() {
  return (
    <Router>
      <div className="container">
        <Switch>
          <Route exact path="/" component={Register} />
        </Switch>
      </div>
    </Router>
  );
}

export default App;
