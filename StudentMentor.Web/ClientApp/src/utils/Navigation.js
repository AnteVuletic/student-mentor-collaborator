const studentBaseUrl = "/home/student";
export const studentMenuTabs = {
  messages: "Messages",
  messagesRoute: `${studentBaseUrl}/messages`,
  finalsPaper: "Finals paper",
  finalsPaperRoute: `${studentBaseUrl}/finals-paper`,
};

const mentorBaseUrl = "/home/mentor";
export const mentorMenuTabs = {
  messages: "Messages",
  students: "Students",
  messagesRoute: `${mentorBaseUrl}/messages`,
  studentsRoute: `${mentorBaseUrl}/students`,
};

const adminBaseUrl = "/home/admin";
export const adminScreenTabs = {
  students: "Students",
  mentors: "Mentors",
  studentRoute: `${adminBaseUrl}/students`,
  mentorsRoute: `${adminBaseUrl}/mentors`,
};

export const navigation = {
  student: [
    {
      label: studentMenuTabs.messages,
      path: studentMenuTabs.messagesRoute,
    },
    {
      label: studentMenuTabs.finalsPaper,
      path: studentMenuTabs.finalsPaperRoute,
    },
  ],
  mentor: [
    {
      label: mentorMenuTabs.messages,
      path: mentorMenuTabs.messagesRoute,
    },
    {
      label: mentorMenuTabs.students,
      path: mentorMenuTabs.studentsRoute,
    },
  ],
  admin: [
    {
      label: adminScreenTabs.students,
      path: adminScreenTabs.studentRoute,
    },
    {
      label: adminScreenTabs.mentors,
      path: adminScreenTabs.mentorsRoute,
    },
  ],
};
