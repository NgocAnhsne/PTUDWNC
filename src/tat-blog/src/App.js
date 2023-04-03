import logo from './logo.svg';
import './App.css';
import Navbar from './Components/Navbar';
import Sidebar from './Components/Sidebar';
import Footer from './Components/Footer';
import Index from './Pages/Index';
import Layout from './Pages/Layout';
import About from './Pages/About';
import Contact from './Pages/Contact';
import RSS from './Pages/RSS';
import {
  BrowserRouter as Router,
  Routes,
  Route
} from 'react-router-dom';


function App() {
  return (
    // <div className="App">
    //   <header className="App-header">
    //     <img src={logo} className="App-logo" alt="logo" />
    //     <p>
    //       Edit <code>src/App.js</code> and save to reload.
    //     </p>
    //     <a
    //       className="App-link"
    //       href="https://reactjs.org"
    //       target="_blank"
    //       rel="noopener noreferrer"
    //     >
    //       Learn React
    //     </a>
    //   </header>
    // </div>
    <div>
      <Router>
        <Navbar/>
        <div className='container-fluid'>
          <div className='row'>
            <div className='col-9'>
            <Routes>
              <Route path='/' element={<Layout />}>
                <Route path='blog' element={<Index />} />
                <Route path='blog/Contact' element={<Contact />} />
                <Route path='blog/About' element={<About />} />
                <Route path='/' element={<Index />} />
                <Route path='blog/RSS' element={<RSS />} />
              </Route>
            </Routes>
            </div>
            <div className='col-3 border-start'>
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </Router>
    </div>
    
  );
}

export default App;
