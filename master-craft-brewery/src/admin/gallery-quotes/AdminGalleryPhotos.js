import React, { useEffect, useState } from 'react'
import { Container, Row, Col } from 'reactstrap'
import {  IoTrash } from "react-icons/io5";

export const AdminGalleryPhotos = (props) => {
    const [isInfo, setIsInfo] = useState(false);
    const [mediaFiles, setMediaFiles] = useState([]);
    useEffect(() => {
        if(props.info !== undefined && props.info){
            setIsInfo(true);
        }
        setMediaFiles(props.mediaFiles);

        return () => {
            setMediaFiles([]);
        }
    }, [props.mediaFiles, props.info])

    function removePhoto(photo) {
        setMediaFiles(mediaFiles.filter(function(mf) { return mf !== photo }))
        if(props.removePh !== undefined){
            props.removePh(photo);
        }
    }

    return (
        <div className={isInfo ? "admin-gallery-photos admin-photos-info" : "admin-gallery-photos admin-photos-edit" }>
                <Container className="admin-gallery-photos-container">
                    <Row xs="3">
                        {mediaFiles.map((item, index) => {
                            return(
                                <Col key={index} >
                                    <div className="admin-gallery-col-container" 
                                        >
                                        {!isInfo && <div className="admin-gallery-photo-icon-container">
                                            <IoTrash className="admin-gallery-photo-icon"
                                              onClick={() => removePhoto(item)} />
                                            Izbriši
                                        </div>}
                                        <img  src={`data:image/jpeg;base64,${item.photoInfo.fileData}`} alt="mcb"
                                            className="admin-gallery-photo"/>
                                    </div>
                                </Col>
                            )
                        })}
                    </Row>
                </Container>
        </div>
    )
}
