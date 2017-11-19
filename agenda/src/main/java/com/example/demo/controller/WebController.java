package com.example.demo.controller;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.servlet.ModelAndView;

@Controller
public class WebController {

	@RequestMapping("/")
	ModelAndView home(ModelAndView modelAndView) {

		modelAndView.setViewName("home");

		return modelAndView;
	}
	   @RequestMapping(value = "/add", method = RequestMethod.GET)
	   public String addPage() {
	      return "add";
	   }
	   @RequestMapping(value = "/view", method = RequestMethod.GET)
	   public String viewPage() {
	      return "view";
	   }
	   @RequestMapping(value = "/edit", method = RequestMethod.GET)
	   public String editPage() {
	      return "edit";
	   }
	   @RequestMapping(value = "/home", method = RequestMethod.GET)
	   public String homePage() {
	      return "home";
	   }
}