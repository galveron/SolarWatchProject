import { useEffect, useState } from 'react';
import { notification } from 'antd';
import { useNavigate } from "react-router-dom";
import './Login.css';

notification.config({
    duration: 2,
    closeIcon: null
})

function Login() {

    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [isDuplicate, setIsDuplicate] = useState(false)
    const [message, setMessage] = useState("")
    const navigate = useNavigate();

    async function loginUser(e) {
        e.preventDefault()
        try {
            const newUser = {
                email: email,
                password: password
            };
            const response = await fetch('http://localhost:5144/Auth/Login', {
                method: "POST",
                credentials: 'include',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newUser),
            });
            if (!response.ok) {
                notification.error({ message: 'Email or password incorrect!' });
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            navigate('/home');
            notification.success({ message: 'Successful login. Welcome!' });
        }
        catch (error) {
            notification.error({ message: `Couldn't register. ${error.message}` });
        }
    }

    return (
        <>
            <section className="vh-100 gradient-custom">
                <div className="container py-5 h-100">
                    <div className="row d-flex justify-content-center align-items-center h-100">
                        <div className="col-12 col-md-8 col-lg-6 col-xl-5">
                            <div className="card bg-dark text-white" style={{ borderradius: "1rem" }}>
                                <div className="card-body p-5 text-center">

                                    <div className="mb-md-5 mt-md-4 pb-5">

                                        <h2 className="fw-bold mb-2 text-uppercase">Login</h2>
                                        <p className="text-white-50 mb-5">Please enter your login and password!</p>

                                        <div className="form-outline form-white mb-4">
                                            <input type="email" id="typeEmailX" className="form-control form-control-lg" onChange={e => setEmail(e.target.value)} />
                                            <label className="form-label" htmlFor="typeEmailX" >Email</label>
                                        </div>

                                        <div className="form-outline form-white mb-4">
                                            <input type="password" id="typePasswordX" className="form-control form-control-lg" onChange={e => setPassword(e.target.value)} />
                                            <label className="form-label" htmlFor="typePasswordX">Password</label>
                                        </div>

                                        <p className="small mb-5 pb-lg-2"><a className="text-white-50" href="#!">Forgot password?</a></p>

                                        <button className="btn btn-outline-light btn-lg px-5" type="submit" onClick={loginUser}>Login</button>

                                        <div className="d-flex justify-content-center text-center mt-4 pt-1">
                                            <a href="#!" className="text-white"><i className="fab fa-facebook-f fa-lg"></i></a>
                                            <a href="#!" className="text-white"><i className="fab fa-twitter fa-lg mx-4 px-2"></i></a>
                                            <a href="#!" className="text-white"><i className="fab fa-google fa-lg"></i></a>
                                        </div>

                                    </div>

                                    <div>
                                        <p className="mb-0">Don't have an account? <a href="/register" className="text-white-50 fw-bold">Sign Up</a>
                                        </p>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </>
    )

}

export default Login