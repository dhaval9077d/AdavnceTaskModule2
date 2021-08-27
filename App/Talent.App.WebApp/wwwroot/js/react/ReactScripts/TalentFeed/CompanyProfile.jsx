import React from 'react';
import Cookies from 'js-cookie'
import { Popup, Icon ,Card,Image} from 'semantic-ui-react'

export default class CompanyProfile extends React.Component {
    constructor(props) {
        super(props);
    }
    
    render() { 
        let name = this.props.companyDetails ? this.props.companyDetails.name : "";
        let email=this.props.companyDetails ? this.props.companyDetails.email : "" ;
        let phone=this.props.companyDetails ? this.props.companyDetails.phone : "" ;
        let location = { city: '', country: '' }
        if (this.props.companyDetails && this.props.companyDetails.location) {
            location = Object.assign({}, this.props.companyDetails.location);
        }
        console.log(location)  
        return(
        <Card>
            <Card.Content textAlign='center'>
                <Image
                    size='tiny'
                    src='https://res.cloudinary.com/crunchbase-production/image/upload/c_lpad,h_170,w_170,f_auto,b_white,q_auto:eco,dpr_1/j2am2blstezkjehwdbzg'
                    circular
                /><br/><br/>
                <Card.Header>{name}</Card.Header>
                <Card.Meta><Icon name='location arrow'/>{location.city}{", "}{location.country}</Card.Meta>
                <Card.Description>
                    We do not have specific skills that we desire.
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Icon name='phone'/>:{phone}
                <br/>
                <Icon name='mail'/>:{email}
            </Card.Content>
        </Card>
        )
    }
}