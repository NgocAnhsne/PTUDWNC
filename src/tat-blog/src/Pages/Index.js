import React, { useEffect, useState } from 'react';
import PostItem from '../Components/PostItem';
import { getPosts } from '../Services/BlogRepository';

const Index = () => {
    const [postList, setPostList] = useState([]);
    useEffect(() => {
        document.title = 'Trang chủ';

        getPosts().then(data => {
            if (data)
                setPostList(data.items);
            else
                setPostList([]);
        })
    }, []);

    if (postList.length > 0) 
        return ( 
            <div className='p-4'> 
                {postList.map((item, index) => { 
                    return ( 
                        <PostItem postItem={item} key={index}/>
                    );
                })}
            </div>
        );
    else return (
        <></>
    );
    // return (
    //     <h1>
    //         Đây là trang chủ
    //     </h1>
    // );

}
export default Index;