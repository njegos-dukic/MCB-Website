import React from 'react';
import NavMenu from './NavMenu';
import {Footer} from './Footer';
import './Layout.css';

function Layout(props) 
{
 return(
      <div>
        <NavMenu />
        <div className="layout-margin-top">
          {props.children}
        </div>
        <Footer/>
      </div>
  );
}

 export default Layout;