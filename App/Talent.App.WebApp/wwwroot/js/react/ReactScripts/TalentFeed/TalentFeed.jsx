import React from 'react';
import ReactDOM from 'react-dom';
import Cookies from 'js-cookie'
import TalentCard from '../TalentFeed/TalentCard.jsx';
import { Loader,Header, Grid, Container } from 'semantic-ui-react';
import CompanyProfile from '../TalentFeed/CompanyProfile.jsx';
import FollowingSuggestion from '../TalentFeed/FollowingSuggestion.jsx';
import { BodyWrapper, loaderData } from '../Layout/BodyWrapper.jsx';

export default class TalentFeed extends React.Component {
    constructor(props) {
        super(props);

        let loader = loaderData
        loader.allowedUsers.push("Employer")
        loader.allowedUsers.push("Recruiter")

        this.state = {
            loadNumber: 5,
            loadPosition: 0,
            feedData: [],
            watchlist: [],
            loaderData: loader,
            loadingFeedData: false,
            companyDetails:
            {
                companyContact:{
                    name:'',
                    phone:'',
                    email:'',
                    firstName:'',
                    lastName:'',
                    location:[]
                },
                primaryContact:[]
            },
            talent:[]
        }
        this.init = this.init.bind(this);
        this.loadEmployerData=this.loadEmployerData.bind(this)
        this.loadTalentData=this.loadTalentData.bind(this)
        this.handleScroll=this.handleScroll.bind(this)

    };
    init() {
        let loaderData = this.state.loaderData;
       // loaderData.allowedUsers.push("Talent");
        loaderData.isLoading = false;
        this.setState({ loaderData, })
        this.loadEmployerData()

    }
    componentDidMount() {
        this.init();
        this.loadTalentData();
        window.addEventListener('scroll', this.handleScroll);
    };
    loadEmployerData() {
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'https://advancetaskprofile.azurewebsites.net/profile/profile/getEmployerProfile',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            //async:true,
            success: function (res) {
                let employerData = null;
                if (res.employer) {
                    employerData = res.employer.companyContact
                    console.log(employerData)
                    this.setState(
                        {
                            companyDetails:employerData
                        }
                    )
                }
            }.bind(this),
            error: function (res) {
                console.log(res.status)
            }
        })
       // this.init();
    } 
    loadTalentData()
    {
        console.log(this.state.position)
        this.setState({loadingFeedData:true})
        var cookies = Cookies.get('talentAuthToken');
        $.ajax({
            url: 'http://advancetaskprofile.azurewebsites.net/profile/profile/getTalent',
            headers: {
                'Authorization': 'Bearer ' + cookies,
                'Content-Type': 'application/json'
            },
            type: "GET",
            contentType: "application/json",
            dataType: "json",
            async:false,
            data:{
                increment: this.state.loadNumber,
                position: this.state.loadPosition
            },
            success: function (res) {
                this.setState(
                    {
                        talent:[...this.state.talent,...res.data],
                        loadingFeedData:false
                    }
                )
              console.log(res.data)
            }.bind(this),
            error: function (res) {
                console.log(res.status)
            }
        })
        this.init()
    }
    handleScroll(event) {
        console.log('HEllo')
        const win = $(window);
        if ((($(document).height() - win.height()) == Math.round(win.scrollTop())) || ($(document).height() - win.height()) - Math.round(win.scrollTop()) == 1) {
            $("#load-more-loading").show();
            this.setState(
                {
                    loadPosition:this.state.loadPosition+5
                }
            )
           this.loadTalentData();
           console.log("bottom" + this.state.position)
        }
    }
    render() {
        return (
            <BodyWrapper reload={this.init} loaderData={this.state.loaderData}>        
                 <div className="ui grid talent-feed container">
                    <div className="four wide column">
                        <CompanyProfile companyDetails={this.state.companyDetails}/>
                    </div>
                    <div className="eight wide column">
                        <TalentCard talentDetail={this.state.talent}/>
                    </div>
                    <div className="four wide column">
                        <div className="ui card">
                            <FollowingSuggestion />
                        </div>
                    </div>
                    
                </div>
                
            </BodyWrapper>
        )
       
    }
}