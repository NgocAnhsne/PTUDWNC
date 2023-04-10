import React, { useEffect } from 'react';
import  Button  from 'react-bootstrap/Button';

const NotFound = () => {
    useEffect(() => {
        document.title = 'Trang giới thiệu';
        }, []);
    return (
    <div className='container-fluid d-flex flex-column justify-content-center align-items-center pt-5 pb-5'>
        <h1 className='fw-bolder display-1'>
            400
        </h1>
        <h2><span className='text-danger'>Chà!</span>Không tìm thấy trang rồi.</h2>
        <p className='text-black-50'>Trang mà bạn đang tìm không tồn tại</p>
        <Button className='btn btn-primary'>
            Về trang chủ thôi!
        </Button>
    </div>
    
    );
}
export default NotFound;