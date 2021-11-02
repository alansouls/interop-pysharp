import { MenuItems } from "./MenuItems";
import './Navbar.css';
import React, { useState } from 'react';
import { Button } from "../Button/Button";

function Navbar(clicked) {
    var [clicked, setClicked] = useState(false);
    return (
        <nav className="NavbarItems">
            <h1 className="navbar-logo">React <i className="fab fa-react"></i></h1>
            <div className="menu-icon" onClick={() => setClicked(!clicked)}>
                <i className={clicked ? 'fas fa-times' : 'fas fa-bars'}></i>
            </div>
            <ul className={clicked ? 'nav-menu active' : 'nav-menu'}>
                {
                MenuItems.map((item, index) => {
                    return (
                        <li key={index}><a className={item.cName} href={item.url}>{item.title}</a></li>
                    );
                })
                }
            </ul>
            <Button>
                Sign up
            </Button>
        </nav>
    );
}

export default Navbar;