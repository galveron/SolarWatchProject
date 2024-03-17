import { useEffect, useState } from 'react';


function Register() {

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [email, setEmail] = useState("");
    const [isDuplicate, setIsDuplicate] = useState(false)
    const [success, setSuccess] = useState(false)

    async function registerUser(e) {
        e.preventDefault()
        setIsDuplicate(false)
        try {
            const newUser = {
                email: email,
                username: username,
                password: password,
            };

            const response = await fetch('http://localhost:5144/Auth/Register', {
                method: "POST",
                headers: {
                    "Content-type": "application/json",
                    "Accept": "text/plain",
                },
                body: JSON.stringify(newUser),
            });

            const result = await response.json()

            if (!response.ok) {
                throw error
            }
            if (Object.keys(result)[0] == "DuplicateEmail") {
                setIsDuplicate(true)
            }
            setUsername("");
            setEmail("");
            setPassword("");
            setSuccess(true);
        }
        catch {
            throw error
        }

    }

    return (
        <>
            {success ?
                <section className="vh-100 gradient-custom">
                    <div className="container py-5 h-100">
                        <div className="row d-flex justify-content-center align-items-center h-100">
                            <div className="col-12 col-md-8 col-lg-6 col-xl-5">
                                <div className="card bg-dark text-white" style={{ borderradius: "1rem" }}>
                                    <div className="card-body p-5 text-center">

                                        <div className="mb-md-5 mt-md-4 pb-5">

                                            <h2 className="fw-bold mb-2 text-uppercase">Success</h2>
                                            <p className="text-white-50 mb-5">Your registration was successful! No you can log in</p>
                                            <div />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>
                :
                <section className="main-container gradient-custom">
                    <div className="container py-5 h-100">
                        <div className="row d-flex justify-content-center align-items-center h-100">
                            <div className="col-12 col-md-8 col-lg-6 col-xl-5">
                                <div className="card bg-dark text-white" style={{ borderradius: "1rem" }}>
                                    <div className="card-body p-5 text-center">

                                        <div className="mb-md-5 mt-md-4 pb-5">

                                            <h2 className="fw-bold mb-2 text-uppercase">Register</h2>
                                            <p className="text-white-50 mb-5">Please enter your data!</p>

                                            <div className="form-outline form-white mb-4">
                                                <input type="email" id="typeEmailX" className="form-control form-control-lg" onChange={e => setEmail(e.target.value)} />
                                                <label className="form-label" htmlFor="typeEmailX" >Email</label>
                                            </div>

                                            <div className="form-outline form-white mb-4">
                                                <input type="username" id="typeUsernameX" className="form-control form-control-lg" onChange={e => setUsername(e.target.value)} />
                                                <label className="form-label" htmlFor="typeUsernameX">User name</label>
                                            </div>

                                            <div className="form-outline form-white mb-4">
                                                <input type="password" id="typePasswordX" className="form-control form-control-lg" onChange={e => setPassword(e.target.value)} />
                                                <label className="form-label" htmlFor="typePasswordX">Password</label>
                                            </div>

                                            <button className="btn btn-outline-light btn-lg px-5" type="submit" onClick={registerUser}>Register</button>

                                            <div className="d-flex justify-content-center text-center mt-4 pt-1">
                                                <a href="#!" className="text-white"><i className="fab fa-facebook-f fa-lg"></i></a>
                                                <a href="#!" className="text-white"><i className="fab fa-twitter fa-lg mx-4 px-2"></i></a>
                                                <a href="#!" className="text-white"><i className="fab fa-google fa-lg"></i></a>
                                            </div>

                                        </div>

                                        <div>
                                            <p className="mb-0">Do have an account? <a href="/login" className="text-white-50 fw-bold">Log in</a>
                                            </p>
                                        </div>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </section>}

            {isDuplicate === true ?
                (<p>This email is already registered. Please try again or log in!</p>)
                : (<p>Registration was succesful!</p>)
            }
        </>
    )

}

export default Register