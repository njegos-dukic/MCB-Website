import React, { useEffect, useState } from 'react'
import { Button, Form, FormGroup, Label, Input, Container, Row, Col } from 'reactstrap';
import { AdminGalleryPhotos } from './AdminGalleryPhotos';
import { IsValidString } from '../../error-handling/InputValidator';
import { INVALID_GALLERY_NAME } from '../../constants/Messages';


export const AdminAddEditInfoGallery = (props) => {
    const [galleryId, setGalleryId] = useState(0);
    const [galleryName, setGalleryName] = useState('');
    const [galleryDescription, setGalleryDescription] = useState('');
    const [nameError, setNameError] = useState('');
    var mediaFilesToSend;
    const [photoInfos, setPhotoInfos] = useState([]);
    const [edit, setEdit] = useState(false);
    const [add, setAdd] = useState(false);
    const [info, setInfo] = useState(false);
    const [disabled, setDisabled] = useState(false);
    
    const uploadMultipleFiles = (files) => {
        setPhotoInfos(files);
    }
    

    useEffect(() => {
        if(props.gallery !== undefined && props.gallery !== null) { 
            setGalleryName(props.gallery.name);
            setGalleryDescription(props.gallery.description);
            setGalleryId(props.gallery.galleryId)
        }
        if(props.add !== undefined && props.add){
            setAdd(true);
        } else if(props.edit !== undefined && props.edit) {
            setEdit(true);
        } else if(props.info !== undefined && props.info) {
            setInfo(true);
            setDisabled(true)
        }

        return () => {
            setGalleryName('');
            setGalleryDescription('');
            setGalleryId(0);
        }
    }, [props.add, props.edit, props.info, props.gallery]);


    const AdminMultiPhotosBrowser = () => {
        return (
            <div className="admin-photos-browser-container">
                <Container>
                    <Row lg="1">
                        <Col  className="admin-add-photo-browse">
                            <Input type="file" id="input-photos"  multiple accept="image/*" 
                                 onChange={e => uploadMultipleFiles(e.target.files)} />
                            <Label>Broj selektovanih slika je {photoInfos.length}</Label>
                        </Col>
                    </Row>
                </Container>
            </div>
        )
    }


    const FinalGalleryAdminForm = () => {
        if(props.edit !== undefined && props.edit === true){
            mediaFilesToSend = props.gallery.mediaFiles;
            return(
                <>
                    <AdminGalleryPhotos mediaFiles={mediaFilesToSend} removePh={(mf) => removePhoto(mf)} />
                    <AdminMultiPhotosBrowser />
                </>
            )
        }
        else {
            mediaFilesToSend = props.gallery.mediaFiles;
            return(
                <AdminGalleryPhotos info={true} mediaFiles={mediaFilesToSend} />
            )
        }
    }

    const isValidModel = () => {
        let errorIndicator = true;
        if (!IsValidString(galleryName)) {
            setNameError(INVALID_GALLERY_NAME);
            errorIndicator = false;
        }
        else
            setNameError('');
        return errorIndicator;
    }

    async function func() {
        if(isValidModel()){
            if(props.add !== undefined && props.add === true) {
                let galleryData = {
                    'name': galleryName,
                    'description': galleryDescription
                }
                await props.func(galleryData);
            }
            else if(props.edit !== undefined && props.edit === true) {
                let galleryData = {
                    'galleryId': galleryId,
                    'name': galleryName,
                    'description': galleryDescription,
                    'mediaFiles': mediaFilesToSend.map(detailed => {
                        return {
                            'mediaFileId': detailed.mediaFileId
                        }
                    })
                }
                await props.func(galleryData);
                if(props.addPhotos !== undefined && photoInfos.length > 0){
                    const formData = new FormData();

                    for(let i = 0; i < photoInfos.length; ++i) {
                        formData.append('images', photoInfos[i])
                    }

                    await props.addPhotos(formData, galleryId);
                }
            }
            props.toggle();
        }
    }

    function removePhoto(mediaFile) {
        mediaFilesToSend = mediaFilesToSend.filter(function(mf) { return mf !== mediaFile });
    }

    return (
        <Container>
            <Row xs="1" sm="1" md="1" lg={add ? "1" : "2"}>
                <Col>
                    <Form>
                        <FormGroup>
                            <Label>Naziv</Label>
                                <Input type="text" defaultValue={galleryName} 
                                    onInput={e => setGalleryName(e.target.value)}
                                    disabled={(disabled)? "disabled" : ""} />
                            <Label className="action-input-error">{nameError}</Label>
                        </FormGroup>
                        <FormGroup>
                            <Label>Opis</Label>
                                <Input type="textarea" defaultValue={galleryDescription}
                                onInput={e => setGalleryDescription(e.target.value)}
                                disabled={(disabled)? "disabled" : ""} />
                        </FormGroup>
                    </Form>
                </Col>
                {   
                    (edit || info) &&
                    <Col>
                            <FinalGalleryAdminForm />
                    </Col>
                }
                {
                    (edit || add) && 
                    <Col>
                        <Button className="quote-gallery-save-button" color="warning" 
                            onClick={async () => await  func()}>Sačuvaj</Button>
                    </Col>
                }
            </Row>
        </Container>
    )
}
