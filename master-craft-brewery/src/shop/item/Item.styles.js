import styled from 'styled-components';

export const Wrapper = styled.div`
    overflow: hidden;
    color: white;
    background: #2b2b2b;
    display: flex;
    justify-content: center;
    flex-direction: column;
    width: 75%;
    border: 1px solid white;
    border-radius: 20px;
    height: 100%;
    margin-left: 25px;

    button {
        background: #1a1a1a;
        color: white;
        border-radius: 0 0 20px 20px;
    }

    button:hover {
        background: #0a0a0a;
    }

    img {
        background: #1c1c1c;
        height: 400px;
        object-fit: cover;
        border-radius: 20px 20px 0 0;
    }

    div {
        font-family: Arial, Hevletica, sans-serif;
        padding: 1rem;
        height: 100%;
    }
`;