import React from 'react'
import { GoogleApiWrapper, Map, Marker } from 'google-maps-react';
import logo from "../assets/PIN.png";
 
const darkMode = 
{
  styles: [
    { elementType: "geometry", stylers: [{ color: "#242f3e" }] },
    { elementType: "labels.text.stroke", stylers: [{ color: "#242f3e" }] },
    { elementType: "labels.text.fill", stylers: [{ color: "#746855" }] },
    {
      featureType: "administrative.locality",
      elementType: "labels.text.fill",
      stylers: [{ color: "#d59563" }],
    },
    {
      featureType: "poi",
      elementType: "labels.text.fill",
      stylers: [{ color: "#d59563" }],
    },
    {
      featureType: "poi.park",
      elementType: "geometry",
      stylers: [{ color: "#263c3f" }],
    },
    {
      featureType: "poi.park",
      elementType: "labels.text.fill",
      stylers: [{ color: "#6b9a76" }],
    },
    {
      featureType: "road",
      elementType: "geometry",
      stylers: [{ color: "#38414e" }],
    },
    {
      featureType: "road",
      elementType: "geometry.stroke",
      stylers: [{ color: "#212a37" }],
    },
    {
      featureType: "road",
      elementType: "labels.text.fill",
      stylers: [{ color: "#9ca5b3" }],
    },
    {
      featureType: "road.highway",
      elementType: "geometry",
      stylers: [{ color: "#746855" }],
    },
    {
      featureType: "road.highway",
      elementType: "geometry.stroke",
      stylers: [{ color: "#1f2835" }],
    },
    {
      featureType: "road.highway",
      elementType: "labels.text.fill",
      stylers: [{ color: "#f3d19c" }],
    },
    {
      featureType: "transit",
      elementType: "geometry",
      stylers: [{ color: "#2f3948" }],
    },
    {
      featureType: "transit.station",
      elementType: "labels.text.fill",
      stylers: [{ color: "#d59563" }],
    },
    {
      featureType: "water",
      elementType: "geometry",
      stylers: [{ color: "#17263c" }],
    },
    {
      featureType: "water",
      elementType: "labels.text.fill",
      stylers: [{ color: "#515c6d" }],
    },
    {
      featureType: "water",
      elementType: "labels.text.stroke",
      stylers: [{ color: "#17263c" }],
    },
  ],
}

class MapContainer extends React.Component 
{
    shouldComponentUpdate(nextProps, nextState) 
    {
      if (this.props.markers !== nextProps.markers)
        return true

      if (this.state === nextState)
        return false

      return true
    }

    render() {
      return (  
        <Map google={ this.props.google } zoom={ 14 } styles={ darkMode.styles } initialCenter={{ lat: 44.763778, lng: 17.1978144}}>
          <Marker position={{ lat: 44.763778, lng: 17.1978144}} onClick={ this.onMarkerClick } name={'MCB'} icon={ logo }/> 

          { 
            this.props.markers.map((marker, index) => {
              return (
                <Marker
                  onClick={ this.onMarkerClick }
                  key={ index } 
                  name={ marker.name }
                  position={ marker.position } 
                />)})
          }
        </Map>
      )
    }
}
 

export default GoogleApiWrapper ({apiKey: (process.env.REACT_APP_MCB_GOOGLE_MAPS_API_KEY)}) (MapContainer)