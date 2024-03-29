﻿import './NavBar.css';
import { Link } from "react-router-dom";
import useLogout from "../hooks/useLogout"
import useRefreshToken from '../hooks/useRefreshToken';
import useAuth from '../hooks/useAuth';
import NavDropdown from 'react-bootstrap/NavDropdown';


function NavBar() {

    /*    const { loginWithRedirect, isAuthenticated } = useAuth0();*/
    const logout = useLogout();
    const { auth } = useAuth();


    return (

        <div id='main-nav'>
            <nav className="navbar navbar-expand-lg navbar-dark bg-primary">
                <div className="navbar-collapse collapse w-100 order-1 order-md-0 dual-collapse2">
                    <ul className="navbar-nav me-auto">

                        {/* Not Logged in TABS*/}
                        {(!auth?.roles)
                            ?
                            <>
                                <li className="nav-item text-light">
                                    <Link className="nav-link text-light" to="/" >Home</Link>
                                </li>
                                <li className="nav-item">
                                    <Link className="nav-link" to="EShopList" >Browse Certifications</Link>
                                </li>
                            </>
                            :
                            <li></li>

                        }


                        {/* ADMIN NAVBAR TABS*/}
                        {auth?.roles?.find(roles => roles.includes("Admin"))
                            ?
                            <ul className="navbar-nav ms-auto">
                                <li className="nav-item">
                                    <Link className="nav-link text-light" to="/" >Home</Link>
                                </li>
                                <NavDropdown title="Certification Manager" id="navbarScrollingDropdown">
                                    <Link className="nav-link" to="AdminUI/Certificates" >
                                        <div className='dropdown-item' >Certificates</div>
                                    </Link>
                                    <Link className="nav-link " to="AdminUI/Exams" >
                                        <div className='dropdown-item'>Exams</div>
                                    </Link>
                                    <Link className="nav-link " to="AdminUI/QuestionList" >
                                        <div className='dropdown-item'>Questions</div>
                                    </Link>
                                </NavDropdown>
                                <li className="nav-item">
                                    <Link className="nav-link" to="AdminUI/Candidates" >Candidates</Link>
                                </li>
                              
                                <li className="nav-item"><Link className="nav-link" to="AdminUI/AssignMarkers" >Assign Markers</Link></li>
                            </ul>
                            : <li></li>
                        }

                        {/* CANDIDATE NAVBAR TABS*/}
                        {auth?.roles?.find(roles => roles?.includes("Candidate"))
                            ? <div className='navbar-collapse collapse w-100 order-3 dual-collapse2'>
                                <ul className="navbar-nav ms-auto">
                                    <li className="nav-item">
                                        <Link className="nav-link text-light" to="/" >Home</Link>
                                    </li>
                                    <li className="nav-item">
                                        <Link className="nav-link" to="EShopList" >Browse Certifications</Link>
                                    </li>
                                    {/* <li  className="nav-item">
                                    <Link className="nav-link text-light" to="CandidateUI" >Exams</Link>
                                    </li> */}
                                    <NavDropdown title="Exams" id="navbarScrollingDropdown">
                                        <Link className="nav-link text-light" to="/Exams/UpcomingExams" >
                                            <div className='dropdown-item' >Upcoming Exams</div>
                                        </Link>
                                        {/* <Link className="nav-link text-light" to="CandidateUI" >
                                            <div className='dropdown-item'>Take your Exam Now!</div>
                                        </Link> */}
                                        <Link className="nav-link text-light" to="/Exams/SchedulerMenu" >
                                            <div className='dropdown-item'>Schedule your Exam!</div>
                                        </Link>
                                        <NavDropdown.Divider />
                                        <Link className="nav-link text-light" to="/Exams/VouchersList" >
                                            <div className='dropdown-item'>Vouchers</div>
                                        </Link>
                                    </NavDropdown>
                                    <li className="nav-item">
                                        <Link className="nav-link " to="MyCertificatesList" >My Certificates</Link>
                                    </li>
                                </ul>
                            </div>
                            : <li></li>
                        }

                        {/* MARKER NAVBAR TABS*/}
                        {auth?.roles?.find(roles => roles?.includes("Marker"))
                            ? <>
                                <li className="nav-item"><Link className="nav-link text-light" to="/" >Home</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="MarkerUI/UnmarkedExamList" >Unmarked Exams</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="MarkerUI/MarkedExamsList" >Marked Exams</Link></li>
                            </>
                            : <li></li>
                        }

                        {/* QualityControl NAVBAR TABS*/}
                        {auth?.roles?.find(roles => roles?.includes("QualityControl"))
                            ? <>
                                <li className="nav-item"><Link className="nav-link text-light" to="/" >Home</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="QualityControlUI/CandidateList" >Candidates</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="QualityControlUI/ExamList" >Exams</Link></li>
                                <li className="nav-item"><Link className="nav-link" to="QualityControlUI/CertificateList" >Certificates</Link></li>
                            </>
                            : <li></li>
                        }
                    </ul>
                </div>
                <div className="mx-auto order-0">
                    <a className="navbar-brand mx-auto">Certy</a>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".dual-collapse2">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                </div>

                {/* LOGIN - LOGOUT TOGGLE NAVBAR*/}
                {auth?.userName ? (
                    // navbar-collapse collapse w-100 order-1 order-md-0 dual-collapse2
                    <div className="navbar-collapse collapse w-100 order-3 dual-collapse2">
                        <ul className="navbar-nav ms-auto">
                            <li className="nav-item">
                                <span className="nav-link text-light">Welcome {auth.userName}</span> {/*(role:{auth.roles})*/}
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-light" onClick={() => logout()}>Logout</Link>
                            </li>
                        </ul>
                    </div>

                ) : (

                    <div className="navbar-collapse collapse w-100 order-3 dual-collapse2">
                        <ul className="navbar-nav ms-auto">
                            <li className="nav-item">
                                <Link className="nav-link text-light" to="Register" >Register</Link>
                            </li>
                            <li className="nav-item">
                                <Link className="nav-link text-light" to="Login" >Login</Link>
                            </li>
                        </ul>
                    </div>

                )}
            </nav>
        </div>

    );
}

export default NavBar;




