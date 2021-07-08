import React, { useEffect, useState } from 'react'
import { Button, Form, FormGroup, Label, Input } from 'reactstrap';
import { INVALID_QUOTE_TEXT } from '../../constants/Messages';
import { IsValidString } from '../../error-handling/InputValidator';



export const AdminAddEditQuote = (props) => {
    const [quoteId, setQuoteId] = useState(0);
    const [author, setAuthor] = useState('');
    const [quoteText, setQuoteText] = useState('');
    const [quoteTextError, setQuoteTextError] = useState('');


    useEffect(() => {
        if(props.quote !== undefined && props.quote !== null) {
            setAuthor(props.quote.author);
            setQuoteText(props.quote.quoteText);
            setQuoteId(props.quote.quoteId);
        }

        return() => {
            setAuthor('');
            setQuoteText('');
        }
    }, [props.quote]);

    const isValidModel = () => {
        let errorIndicator = true;
        if (!IsValidString(quoteText)) {
            setQuoteTextError(INVALID_QUOTE_TEXT);
            errorIndicator = false;
        }
        else
            setQuoteTextError('');
        return errorIndicator;
    }

    async function func() {
        if(isValidModel()) {
            let quoteData = {
                'quoteId': quoteId,
                'author': author,
                'quoteText': quoteText
            }
            await props.func(quoteData);
            props.toggle();
        }
    }

    return (
        <>
            <Form className="admin-quotes-form">
                <FormGroup>
                    <Label>Autor</Label>
                    <Input type="text" defaultValue={author}
                        onInput={e => setAuthor(e.target.value)} />
                </FormGroup>
                <FormGroup>
                    <Label>Citat</Label>
                    <Input type="textarea" defaultValue={quoteText}
                        onInput={e => setQuoteText(e.target.value)} ></Input>
                    <Label className="action-input-error">{quoteTextError}</Label>
                </FormGroup>
                <FormGroup>
                    <Button className="quote-gallery-save-button" color="warning" onClick={async () => await func()} >Sačuvaj</Button>
                </FormGroup>
            </Form>
        </>
    )
}
