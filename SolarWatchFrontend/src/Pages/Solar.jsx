import { useEffect, useState } from 'react';
import { notification } from 'antd';
import { useNavigate } from "react-router-dom";
import '../index.css';
import { Modal, Ripple, initMDB } from "mdb-ui-kit";

initMDB({ Modal, Ripple });

function Solar() {
    const [loading, setLoading] = useState(true);
    const [data, setData] = useState([]);
    const navigate = useNavigate();
    const [inputDate, setInputDate] = useState("")
    const [inputCity, setInputCity] = useState("")
    const [submit, setSubmit] = useState(false)

    async function fetchData(e) {
        try {
            const response = await fetch('http://localhost:5144/SolarWatch/GetAll', {
                method: 'GET',
                credentials: 'include'
            })
            return await response.json();
        }
        catch (error) {
            notification.error({ message: `Couldn't load data. ${error.message}` });
        }
    }

    useEffect(() => {
        fetchData()
            .then(datas => setData(datas), setLoading(false))
    }, [])

    async function handleSubmit(e) {
        e.preventDefault()
        try {
            let newData = { date: inputDate.toString(), city: inputCity }
            const response = await fetch('http://localhost:5144/SolarWatch/NewRequest', {
                method: "POST",
                headers: {
                    "Content-type": "application/json",
                    "Accept": "*/*",
                },
                credentials: 'include',
                body: JSON.stringify(newData)
            })
            return await response.json();

        }
        catch (error) {
            notification.error({ message: `Couldn't load data. ${error.message}` });
        }
    }

    return (
        <>
            {loading ? <h1>Loading...</h1> :
                <>
                    <section className="vh-100 gradient-custom">
                        <div className="container py-5 h-100">
                            <div className="row d-flex justify-content-center align-items-center h-100">
                                <table className="table table-striped align-middle mb-0 table-dark">
                                    <thead className="thead-light">
                                        <tr className='tableButton'>
                                            <th>
                                                <button type="button" className="btn btn-primary" data-toggle="modal" data-target="#exampleModal">
                                                    Fetch new
                                                </button>

                                            </th>
                                        </tr>
                                        <tr>
                                            <th>Date</th>
                                            <th>City</th>
                                            <th>Sun rise</th>
                                            <th>Sun set</th>
                                            <th>Note</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {data != null ?
                                            data.map(line => {
                                                return (
                                                    <tr>
                                                        <td>
                                                            <div className="d-flex align-items-center">
                                                                <div className="ms-3">
                                                                    <p className="mb-0">{line.date.split('T')[0]}</p>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <p className="fw-normal mb-1">{line.city}</p>
                                                            {/* <p className="text-muted mb-0">IT department</p> */}
                                                        </td>
                                                        <td>
                                                            <span className="fw-normal mb-1">{line.sunRise.split(' ')[1]}</span>
                                                        </td>
                                                        <td><span className="fw-normal mb-1">{line.sunSet.split(' ')[1]}</span></td>
                                                        <td>
                                                            <span className="fw-normal mb-1">{line.description}</span>
                                                        </td>
                                                    </tr>
                                                )
                                            }) :
                                            <tr>
                                                <td>
                                                    <div>No data yet</div>
                                                </td>
                                            </tr>}

                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </section>
                </>}
            <div className="modal fade" id="exampleModal" tabIndex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                <div className="modal-dialog" role="document">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="exampleModalLabel">Modal title</h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div className="modal-body">
                            <div className="mb-md-5 mt-md-4 pb-5">

                                <h2 className="fw-bold mb-2 text-uppercase">Fetch data</h2>
                                <p className="text-white-50 mb-5">Please enter a date (yyyy-mm-dd) and a city!</p>

                                <div className="form-outline form-white mb-4">
                                    <input type="date" id="typeDateX" className="form-control form-control-lg" onChange={e => setInputDate(e.target.value)} />
                                    <label className="form-label" htmlFor="typeDateX">Date</label>
                                </div>

                                <div className="form-outline form-white mb-4">
                                    <input type="city" id="typeCityX" className="form-control form-control-lg" onChange={e => setInputCity(e.target.value)} />
                                    <label className="form-label" htmlFor="typeCityX">City</label>
                                </div>
                                <div className="d-flex justify-content-center text-center mt-4 pt-1">
                                </div>

                            </div>
                        </div>
                        <div className="modal-footer">
                            <button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
                            <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={handleSubmit}>Save changes</button>
                        </div>
                    </div>
                </div>
            </div>
        </>)
}

export default Solar;