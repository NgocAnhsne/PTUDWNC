import React, { useEffect } from 'react';
import  Button  from 'react-bootstrap/Button';

const NotFound = () => {
    useEffect(() => {
        document.title = 'Trang giới thiệu';
        }, []);
    return (
    <div className='container-fluid d-flex flex-column justify-content-center align-items-center pt-5 pb-5'>
        <h1>
            404
        </h1>
        <h2><span className='text-red '>Chà!</span>Không tìm thấy trang rồi.</h2>
        <h5 className='text-black-50'>Trang mà bạn đang tìm không tồn tại</h5>
        <Button className='btn btn-primary'>
            Về trang chủ thôi!
        </Button>
    </div>
    
    );
}
export default NotFound;