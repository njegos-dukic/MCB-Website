import styled from 'styled-components';
import IconButton from '@material-ui/core/IconButton';

export const Wrapper = styled.div`
    margin: -8px;
    overflow: hidden;
    background: #2b2b2b;
    position: relative;
`;

export const StyledButton = styled(IconButton)`
    position: absolute;
    z-index: 100;
    right: 31px;
    top: 15px;
`;