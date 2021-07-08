import React, { useEffect, useState } from 'react'
import { Container, Row, Col,
    Modal, ModalHeader, ModalBody, ModalFooter
} from 'reactstrap'
import { GALLERIES_PREFIX_ROUTE } from '../constants/ApiRoutes';
import axios from 'axios';
import { IoArrowBack, IoArrowForward } from "react-icons/io5";

export const GalleryPage = (props) => {
    const [gallery, setGallery] = useState({});
    const [mediaFiles, setMediaFiles] = useState([]);

    useEffect(() => {
        const cancelTokenGallery = axios.CancelToken;
        const sourceGallery = cancelTokenGallery.source();

        async function fetchData() {
            await axios.get(`${GALLERIES_PREFIX_ROUTE}/${props.id}`, { cancelToken: sourceGallery.token })
                .then((response) => {
                    setGallery(response.data)
                    setMediaFiles(response.data.mediaFiles);
                });
        }
        fetchData();
        
        return () => {
            setGallery({});
            setMediaFiles([]);
            sourceGallery.cancel();
        }
    }, [props.id]);
        
    const [modal, setModal] = useState(false);
  
    const toggle = () => setModal(!modal);

    const [image, setImage] = useState({});
    const setImageForModal = (imgData, imgId) => {
        setImage({ 'data': imgData, 'id': imgId});
        toggle();
    }

    const goImageForward = () => {
        var found = false;
        mediaFiles.every(mf => {
            if(mf.mediaFileId === image.id) 
                found = true;

            if(found && mf.mediaFileId !== image.id) {
                setImage({ 'data': mf.photoInfo.fileData, 'id': mf.mediaFileId });
                return false;
            }
            return true
        })
    }

    const goImageBackward = () => {
        var found = false;
        var foundId = 0;
        mediaFiles.every(mf => {
            if(mf.mediaFileId === image.id)
                found = true;
            else
                foundId = mf.mediaFileId;

            if(found === true)
                return false;

            return true
        });
        mediaFiles.every(mf => {
            if(mf.mediaFileId === foundId) {
                setImage({ 'data': mf.photoInfo.fileData, 'id': mf.mediaFileId });
                return false;
            }
            return true
        })
    }
    

    return (
        <div className="gallery-and-quotes-background">
            <h1 className="gallery-page-h gallery-page-h1">{gallery.name}</h1>
            <h3 className="gallery-page-h gallery-page-h3">{gallery.description}</h3>
            <div className="gallery-images-container">
                <Container>
                    <Row xs="1" sm="2" md="3" lg="4" className="gallery-row">
                        {mediaFiles.map((item, index) => {
                            return(
                                <Col key={index} sm="6" md="4" lg="3" className="gallery-col">
                                    <img  src={`data:image/jpeg;base64,${item.photoInfo.fileData}`} alt="mcb"
                                        onClick={() => setImageForModal(item.photoInfo.fileData, item.mediaFileId)} />
                                </Col>
                            )
                        })}
                    </Row>
                </Container>
            </div>
            <Modal isOpen={modal} size="lg" toggle={toggle} className="gallery-modal" unmountOnClose={true}>
                <ModalHeader toggle={toggle} />
                <ModalBody>
                    <img src={`data:image/jpeg;base64,${image.data}`} alt="mcb"/>
                </ModalBody>
                <ModalFooter>
                    <IoArrowBack className="slider-button dark-gray" onClick={() => goImageBackward()} />
                    <IoArrowForward className="slider-button dark-gray" onClick={() => goImageForward()} />
                </ModalFooter>
            </Modal>
        </div>
    )
}