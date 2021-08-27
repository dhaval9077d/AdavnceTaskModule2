import React from 'react';
import ReactPlayer from 'react-player';
import PropTypes from 'prop-types'
import { Button, Icon ,Card,Image, Container,Item,Grid, Header} from 'semantic-ui-react'

export default class TalentCard extends React.Component {
    constructor(props) {
        super(props);
        this.state=
        {
            id:'',
            display:'Video',
            EmployerDetail:[],
        }
       this.onClickVideo=this.onClickVideo.bind(this)
       this.onClickProfile=this.onClickProfile.bind(this)
    };
    onClickVideo()
    {
        this.setState(
            {
                display:null,
                id:null
            }
        )
    }
    onClickProfile(event)
    {
        console.log("Profile" + event.target.id )
        this.setState(
            {
                display:"Profile",
                id:event.target.id
            }
        )
    }
    render()
    {
        const src='https://semantic-ui.com/images/avatar2/large/kristy.png'
        console.log("video")
        let disply;
        return(
            <div>
                {this.props.talentDetail.map((talent) => (
                <Card fluid key={talent.id}>
                <Card.Content>
                <Card.Header><Container textAlign='right'><Icon name='star'/></Container>{talent.name}</Card.Header>
                </Card.Content>
                <Card.Content>
                <Card.Content> 
                   {
                       this.state.display == "Profile" && talent.id == this.state.id
                       ?
                       <Item>
                        <Image src={src} size='small' floated='left' />
                            <Item.Content>
                                <Item.Description>
                                    <Header sub> Heading {talent.name}</Header>
                                    <Header as='h5' sub>
                                    Current Employer
                                        <Header.Subheader>
                                        {talent.currentEmployment}
                                        </Header.Subheader>
                                    </Header>
                                    <Header as='h5' sub>
                                    Visa Status
                                        <Header.Subheader>
                                        {talent.visa}
                                        </Header.Subheader>
                                    </Header>
                                    <Header as='h5' sub>
                                    Position
                                        <Header.Subheader>
                                        {talent.level}
                                        </Header.Subheader>
                                    </Header>
                                </Item.Description>
                            </Item.Content>
                        </Item>
                       :
                       <ReactPlayer  width='520px'
                        url={talent.videoUrl ? talent.videoUrl : 'https://www.youtube.com/watch?v=ysz5S6PUM-U'}
                        />
                   }
                </Card.Content>
                </Card.Content>
                <Card.Content extra>
                    <Header>
                    <Grid>
                    <Grid.Row columns={4}>
                        <Grid.Column>
                        {
                       this.state.display == "Profile" && talent.id == this.state.id
                       ? <Icon name='video' id={talent.id}  size='large' onClick={this.onClickVideo} />
                       : <Icon name='user' id={talent.id}  size='large' onClick={this.onClickProfile} />
                        }
                        </Grid.Column>
                        <Grid.Column>
                        <Icon name='file pdf outline' value="ResumeDetail" size='large' />
                        </Grid.Column>
                        <Grid.Column>
                        <Icon name='linkedin' value="linkedinDetail" size='large' />
                        </Grid.Column>
                        <Grid.Column>
                        <Icon name='github' value="githubDetail" size='large' />
                        </Grid.Column>
                    </Grid.Row>
                    </Grid>
                    </Header>
                </Card.Content>
                <Card.Content extra>
                <Button basic color='blue' content='C#' />
                </Card.Content>
            </Card> 
                ))}
           </div>
        )
    }
}

