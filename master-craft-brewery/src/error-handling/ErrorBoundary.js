import { React, Component } from 'react';
import ErrorPage from './ErrorPage';

export class ErrorBoundary extends Component {
  constructor(props) {
    super(props);
    this.state = { error: null, errorInfo: null };
  }

  componentDidCatch(error, errorInfo) {
    this.setState({
      error: error,
      errorInfo: errorInfo
    })
  }

  render() {
    if (this.state.errorInfo) {
      return (
        <ErrorPage serverResponse={this.state.error.message} />
      );
    }
    return this.props.children;
  }
}