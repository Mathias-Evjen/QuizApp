using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashCardQuizzes",
                columns: table => new
                {
                    FlashCardQuizId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumOfQuestions = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCardQuizzes", x => x.FlashCardQuizId);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    NumOfQuestions = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.QuizId);
                });

            migrationBuilder.CreateTable(
                name: "FlashCards",
                columns: table => new
                {
                    FlashCardId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    ShowAnswer = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashCards", x => x.FlashCardId);
                    table.ForeignKey(
                        name: "FK_FlashCards_FlashCardQuizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "FlashCardQuizzes",
                        principalColumn: "FlashCardQuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FillInTheBlankQuestions",
                columns: table => new
                {
                    FillInTheBlankId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillInTheBlankQuestions", x => x.FillInTheBlankId);
                    table.ForeignKey(
                        name: "FK_FillInTheBlankQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalRows = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchingQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceQuestions",
                columns: table => new
                {
                    MultipleChoiceId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceQuestions", x => x.MultipleChoiceId);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuizAttempts",
                columns: table => new
                {
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    NumOfCorrectAnswers = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuizAttempts", x => x.QuizAttemptId);
                    table.ForeignKey(
                        name: "FK_QuizAttempts_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RankingQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SequenceQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Question = table.Column<string>(type: "TEXT", nullable: false),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    Answer = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SequenceQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueFalseQuestions",
                columns: table => new
                {
                    TrueFalseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuestionText = table.Column<string>(type: "TEXT", nullable: false),
                    CorrectAnswer = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuizId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueFalseQuestions", x => x.TrueFalseId);
                    table.ForeignKey(
                        name: "FK_TrueFalseQuestions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "QuizId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                columns: table => new
                {
                    OptionId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    IsCorrect = table.Column<bool>(type: "INTEGER", nullable: false),
                    MultipleChoiceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.OptionId);
                    table.ForeignKey(
                        name: "FK_Options_MultipleChoiceQuestions_MultipleChoiceId",
                        column: x => x.MultipleChoiceId,
                        principalTable: "MultipleChoiceQuestions",
                        principalColumn: "MultipleChoiceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FillInTheBlankAttempts",
                columns: table => new
                {
                    FillInTheBlankAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FillInTheBlankId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: true),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FillInTheBlankAttempts", x => x.FillInTheBlankAttemptId);
                    table.ForeignKey(
                        name: "FK_FillInTheBlankAttempts_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchingAttempts",
                columns: table => new
                {
                    MatchingAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchingId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    AmountCorrect = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalRows = table.Column<int>(type: "INTEGER", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: true),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchingAttempts", x => x.MatchingAttemptId);
                    table.ForeignKey(
                        name: "FK_MatchingAttempts_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MultipleChoiceAttempts",
                columns: table => new
                {
                    MultiplechoiceAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MultiplechoiceId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: true),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MultipleChoiceAttempts", x => x.MultiplechoiceAttemptId);
                    table.ForeignKey(
                        name: "FK_MultipleChoiceAttempts_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RankingAttempts",
                columns: table => new
                {
                    RankingAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RankingId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    AmountCorrect = table.Column<int>(type: "INTEGER", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: true),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankingAttempts", x => x.RankingAttemptId);
                    table.ForeignKey(
                        name: "FK_RankingAttempts_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SequenceAttempts",
                columns: table => new
                {
                    SequenceAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SequenceId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<string>(type: "TEXT", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: true),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SequenceAttempts", x => x.SequenceAttemptId);
                    table.ForeignKey(
                        name: "FK_SequenceAttempts_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrueFalseAttempts",
                columns: table => new
                {
                    TrueFalseAttemptId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrueFalseId = table.Column<int>(type: "INTEGER", nullable: false),
                    QuizAttemptId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserAnswer = table.Column<bool>(type: "INTEGER", nullable: false),
                    AnsweredCorrectly = table.Column<bool>(type: "INTEGER", nullable: true),
                    QuizQuestionNum = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrueFalseAttempts", x => x.TrueFalseAttemptId);
                    table.ForeignKey(
                        name: "FK_TrueFalseAttempts_QuizAttempts_QuizAttemptId",
                        column: x => x.QuizAttemptId,
                        principalTable: "QuizAttempts",
                        principalColumn: "QuizAttemptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FillInTheBlankAttempts_QuizAttemptId",
                table: "FillInTheBlankAttempts",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_FillInTheBlankQuestions_QuizId",
                table: "FillInTheBlankQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashCards_QuizId",
                table: "FlashCards",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingAttempts_QuizAttemptId",
                table: "MatchingAttempts",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchingQuestions_QuizId",
                table: "MatchingQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceAttempts_QuizAttemptId",
                table: "MultipleChoiceAttempts",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_MultipleChoiceQuestions_QuizId",
                table: "MultipleChoiceQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Options_MultipleChoiceId",
                table: "Options",
                column: "MultipleChoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_QuizAttempts_QuizId",
                table: "QuizAttempts",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingAttempts_QuizAttemptId",
                table: "RankingAttempts",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_RankingQuestions_QuizId",
                table: "RankingQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_SequenceAttempts_QuizAttemptId",
                table: "SequenceAttempts",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_SequenceQuestions_QuizId",
                table: "SequenceQuestions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueFalseAttempts_QuizAttemptId",
                table: "TrueFalseAttempts",
                column: "QuizAttemptId");

            migrationBuilder.CreateIndex(
                name: "IX_TrueFalseQuestions_QuizId",
                table: "TrueFalseQuestions",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FillInTheBlankAttempts");

            migrationBuilder.DropTable(
                name: "FillInTheBlankQuestions");

            migrationBuilder.DropTable(
                name: "FlashCards");

            migrationBuilder.DropTable(
                name: "MatchingAttempts");

            migrationBuilder.DropTable(
                name: "MatchingQuestions");

            migrationBuilder.DropTable(
                name: "MultipleChoiceAttempts");

            migrationBuilder.DropTable(
                name: "Options");

            migrationBuilder.DropTable(
                name: "RankingAttempts");

            migrationBuilder.DropTable(
                name: "RankingQuestions");

            migrationBuilder.DropTable(
                name: "SequenceAttempts");

            migrationBuilder.DropTable(
                name: "SequenceQuestions");

            migrationBuilder.DropTable(
                name: "TrueFalseAttempts");

            migrationBuilder.DropTable(
                name: "TrueFalseQuestions");

            migrationBuilder.DropTable(
                name: "FlashCardQuizzes");

            migrationBuilder.DropTable(
                name: "MultipleChoiceQuestions");

            migrationBuilder.DropTable(
                name: "QuizAttempts");

            migrationBuilder.DropTable(
                name: "Quizzes");
        }
    }
}
