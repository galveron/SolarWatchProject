import { Outlet, Link } from "react-router-dom";

import './Layout.css';

const Layout = () => (
    <>
        <nav className="navbar navbar-expand-lg navbar-dark gradient-custom">
            <div className="container-fluid">
                <a className="navbar-brand" href="/home">SolarWatch</a>
                <button
                    data-mdb-collapse-init
                    className="navbar-toggler"
                    type="button"
                    data-mdb-target="#navbarNav"
                    aria-controls="navbarNav"
                    aria-expanded="false"
                    aria-label="Toggle navigation"
                >
                    <i className="fas fa-bars"></i>
                </button>
                <div className="collapse navbar-collapse" id="navbarNav">
                    <ul className="navbar-nav">
                        <li className="nav-item">
                            <a className="nav-link active" aria-current="page" href="/home">Home</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" href="/solar">Solar data</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" href="/register">Register</a>
                        </li>
                        <li className="nav-item">
                            <a className="nav-link" href="/login">Login</a>
                        </li>
                        {/* <li className="nav-item">
                                    <a className="nav-link disabled"
                                    >Disabled</a>
                                </li> */}
                    </ul>
                </div>
            </div>
        </nav>
        <main role="main" className="container">

            <div className="starter-template">
                <Outlet />
            </div>

        </main>
    </>
)

export default Layout;